namespace GodotApplePlugins.Generator;

class Program
{
    static int Main(string[] args)
    {
        var inputPath = args.Length > 0 ? args[0] : "../../doc_classes";
        var outputPath = args.Length > 1 ? args[1] : "../GodotApplePlugins.Sharp/Generated";

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
            Console.Error.WriteLine("Usage: GodotApplePlugins.Generator [input-path] [output-path]");
            Console.Error.WriteLine();
            Console.Error.WriteLine("  input-path   Path to doc_classes directory with XML files");
            Console.Error.WriteLine("  output-path  Path to output generated C# files");
            return 1;
        }

        // Parse all XML files
        Console.WriteLine("Parsing XML documentation...");
        var classes = XmlDocParser.ParseDirectory(inputPath);
        Console.WriteLine($"Found {classes.Count} classes");
        Console.WriteLine();

        // Clean output directory
        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
        }
        Directory.CreateDirectory(outputPath);

        // Generate code
        Console.WriteLine("Generating C# code...");
        var generator = new CodeGenerator(outputPath);
        generator.GenerateAll(classes);

        Console.WriteLine();
        Console.WriteLine("Done!");

        return 0;
    }
}
