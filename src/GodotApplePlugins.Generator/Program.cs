using System.Net.Http.Json;
using System.Text.Json;

namespace GodotApplePlugins.Generator;

class Program
{
    private const string UpstreamRepo = "migueldeicaza/GodotApplePlugins";
    private const string DocClassesPath = "doc_classes";
    private const string DefaultBranch = "main";

    static async Task<int> Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "update-docs")
        {
            var outputPath = args.Length > 1 ? args[1] : "doc_classes";
            return await UpdateDocs(outputPath);
        }

        if (args.Length > 0 && args[0] == "extract-callbacks")
        {
            var inputPath = args.Length > 1 ? args[1] : "doc_classes";
            var outputPath = args.Length > 2 ? args[2] : "callback-signatures.json";
            return ExtractCallbacks(inputPath, outputPath);
        }

        return Generate(args);
    }

    private const string CallbackSignaturesFileName = "callback-signatures.json";

    static int Generate(string[] args)
    {
        var inputPath = args.Length > 0 ? args[0] : "doc_classes";
        var outputPath = args.Length > 1 ? args[1] : "GodotApplePlugins.NET/Generated";

        // Resolve relative paths
        inputPath = Path.GetFullPath(inputPath);
        outputPath = Path.GetFullPath(outputPath);

        Console.WriteLine($"Input:  {inputPath}");
        Console.WriteLine($"Output: {outputPath}");
        Console.WriteLine();

        if (!Directory.Exists(inputPath))
        {
            Console.Error.WriteLine($"Error: Input directory not found: {inputPath}");
            Console.Error.WriteLine();
            PrintUsage();
            return 1;
        }

        // Parse all XML files
        Console.WriteLine("Parsing XML documentation...");
        var classes = XmlDocParser.ParseDirectory(inputPath);
        Console.WriteLine($"Found {classes.Count} classes");

        // Load callback signatures if available
        var callbackSigPath = Path.Combine(Path.GetDirectoryName(inputPath) ?? ".", CallbackSignaturesFileName);
        CallbackSignaturesFile? callbackSignatures = null;
        if (File.Exists(callbackSigPath))
        {
            Console.WriteLine($"Loading callback signatures from {callbackSigPath}...");
            callbackSignatures = CallbackSignaturesManager.Load(callbackSigPath);
            var validCount = callbackSignatures.Callbacks.Count(c => c.Value.Description?.StartsWith("TODO:") != true);
            Console.WriteLine($"Loaded {validCount} typed callback signatures");
        }
        else
        {
            Console.WriteLine($"No callback signatures file found at {callbackSigPath}");
            Console.WriteLine("Run 'extract-callbacks' to generate one for typed callback support.");
        }

        Console.WriteLine();

        // Clean output directory
        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
        }
        Directory.CreateDirectory(outputPath);

        // Generate code
        Console.WriteLine("Generating C# code...");
        var generator = new CodeGenerator(outputPath, callbackSignatures);
        generator.GenerateAll(classes);

        Console.WriteLine();
        Console.WriteLine("Done!");

        return 0;
    }

    static async Task<int> UpdateDocs(string outputPath)
    {
        outputPath = Path.GetFullPath(outputPath);
        Console.WriteLine($"Downloading XML documentation from {UpstreamRepo}...");
        Console.WriteLine($"Output: {outputPath}");
        Console.WriteLine();

        // Ensure output directory exists
        Directory.CreateDirectory(outputPath);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "GodotApplePlugins.NET-Generator");

        try
        {
            // Get list of files from GitHub API
            var apiUrl = $"https://api.github.com/repos/{UpstreamRepo}/contents/{DocClassesPath}?ref={DefaultBranch}";
            Console.WriteLine($"Fetching file list from GitHub API...");

            var response = await client.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                Console.Error.WriteLine($"Error: Failed to fetch file list: {response.StatusCode}");
                return 1;
            }

            var files = await response.Content.ReadFromJsonAsync<List<GitHubFile>>();
            if (files == null)
            {
                Console.Error.WriteLine("Error: Failed to parse GitHub API response");
                return 1;
            }

            var xmlFiles = files.Where(f => f.name.EndsWith(".xml")).ToList();
            Console.WriteLine($"Found {xmlFiles.Count} XML files");
            Console.WriteLine();

            // Download each file
            int downloaded = 0;
            int failed = 0;

            foreach (var file in xmlFiles)
            {
                var localPath = Path.Combine(outputPath, file.name);
                Console.Write($"  {file.name}... ");

                try
                {
                    var rawUrl = $"https://raw.githubusercontent.com/{UpstreamRepo}/{DefaultBranch}/{DocClassesPath}/{file.name}";
                    var bytes = await client.GetByteArrayAsync(rawUrl);
                    await File.WriteAllBytesAsync(localPath, bytes);
                    Console.WriteLine("OK");
                    downloaded++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAILED: {ex.Message}");
                    failed++;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Downloaded: {downloaded}, Failed: {failed}");

            if (failed > 0)
            {
                Console.Error.WriteLine("Warning: Some files failed to download");
                return 1;
            }

            Console.WriteLine();
            Console.WriteLine("Done! Run the generator to update the C# wrappers:");
            Console.WriteLine("  dotnet run --project src/GodotApplePlugins.Generator");

            return 0;
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Error: Network request failed: {ex.Message}");
            return 1;
        }
    }

    static int ExtractCallbacks(string inputPath, string outputPath)
    {
        inputPath = Path.GetFullPath(inputPath);
        outputPath = Path.GetFullPath(outputPath);

        Console.WriteLine($"Extracting callback signatures...");
        Console.WriteLine($"Input:  {inputPath}");
        Console.WriteLine($"Output: {outputPath}");
        Console.WriteLine();

        if (!Directory.Exists(inputPath))
        {
            Console.Error.WriteLine($"Error: Input directory not found: {inputPath}");
            return 1;
        }

        // Parse all XML files
        var classes = XmlDocParser.ParseDirectory(inputPath);
        Console.WriteLine($"Found {classes.Count} classes");

        // Extract callback signatures from descriptions
        var extracted = CallbackSignaturesManager.ExtractFromClasses(classes);
        Console.WriteLine($"Found {extracted.Callbacks.Count} methods with Callable parameters");
        Console.WriteLine();

        // If output file exists, merge with existing (preserve manual edits)
        if (File.Exists(outputPath))
        {
            Console.WriteLine($"Merging with existing {outputPath}...");
            var existing = CallbackSignaturesManager.Load(outputPath);
            extracted = CallbackSignaturesManager.Merge(existing, extracted);
        }

        // Save
        CallbackSignaturesManager.Save(extracted, outputPath);
        Console.WriteLine($"Saved to: {outputPath}");
        Console.WriteLine();

        // Report status
        var needsReview = extracted.Callbacks.Count(c => c.Value.Description?.StartsWith("TODO:") == true);
        var complete = extracted.Callbacks.Count - needsReview;

        Console.WriteLine($"Complete:     {complete}");
        Console.WriteLine($"Needs review: {needsReview}");

        if (needsReview > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Methods needing manual callback signature definition:");
            foreach (var kvp in extracted.Callbacks.Where(c => c.Value.Description?.StartsWith("TODO:") == true))
            {
                Console.WriteLine($"  - {kvp.Key}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Done! Review the JSON file and fill in any TODO entries.");

        return 0;
    }

    static void PrintUsage()
    {
        Console.Error.WriteLine("Usage:");
        Console.Error.WriteLine();
        Console.Error.WriteLine("  Generate C# wrappers:");
        Console.Error.WriteLine("    dotnet run -- [input-path] [output-path]");
        Console.Error.WriteLine();
        Console.Error.WriteLine("  Update XML documentation from upstream:");
        Console.Error.WriteLine("    dotnet run -- update-docs [output-path]");
        Console.Error.WriteLine();
        Console.Error.WriteLine("  Extract callback signatures from XML descriptions:");
        Console.Error.WriteLine("    dotnet run -- extract-callbacks [input-path] [output-path]");
        Console.Error.WriteLine();
        Console.Error.WriteLine("Arguments:");
        Console.Error.WriteLine("  input-path   Path to doc_classes directory with XML files");
        Console.Error.WriteLine("  output-path  Path for output (generated C#, downloaded XML, or callback JSON)");
    }

    // GitHub API response model
    private record GitHubFile(string name, string path, string download_url);
}
