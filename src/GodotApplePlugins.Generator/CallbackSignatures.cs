using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace GodotApplePlugins.Generator;

/// <summary>
/// Represents a callback parameter type extracted from XML documentation.
/// </summary>
public class CallbackParameter
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
}

/// <summary>
/// Represents the signature of a Callable callback for a specific method.
/// </summary>
public class CallbackSignature
{
    [JsonPropertyName("parameters")]
    public List<CallbackParameter> Parameters { get; set; } = new();

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

/// <summary>
/// Maps class.method to callback signatures.
/// </summary>
public class CallbackSignaturesFile
{
    [JsonPropertyName("$schema")]
    public string Schema { get; set; } = "./callback-signatures.schema.json";

    [JsonPropertyName("callbacks")]
    public Dictionary<string, CallbackSignature> Callbacks { get; set; } = new();
}

/// <summary>
/// Handles loading, saving, and extracting callback signatures.
/// </summary>
public static class CallbackSignaturesManager
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Loads callback signatures from a JSON file.
    /// </summary>
    public static CallbackSignaturesFile Load(string path)
    {
        if (!File.Exists(path))
        {
            return new CallbackSignaturesFile();
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<CallbackSignaturesFile>(json, JsonOptions) ?? new CallbackSignaturesFile();
    }

    /// <summary>
    /// Saves callback signatures to a JSON file.
    /// </summary>
    public static void Save(CallbackSignaturesFile signatures, string path)
    {
        var json = JsonSerializer.Serialize(signatures, JsonOptions);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Extracts callback signatures from parsed classes by analyzing method descriptions.
    /// </summary>
    public static CallbackSignaturesFile ExtractFromClasses(List<GdClass> classes)
    {
        var result = new CallbackSignaturesFile();

        foreach (var gdClass in classes)
        {
            foreach (var method in gdClass.Methods)
            {
                // Check if method has a Callable parameter
                var callableParam = method.Parameters.FirstOrDefault(p => p.Type == "Callable");
                if (callableParam == null)
                    continue;

                var key = $"{gdClass.Name}.{method.Name}";
                var signature = ExtractSignatureFromDescription(method.Description);

                if (signature != null)
                {
                    result.Callbacks[key] = signature;
                }
                else
                {
                    // Add a placeholder for manual completion
                    result.Callbacks[key] = new CallbackSignature
                    {
                        Description = $"TODO: Extract from description: {method.Description}",
                        Parameters = new List<CallbackParameter>
                        {
                            new() { Name = "result", Type = "Variant" }
                        }
                    };
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Attempts to extract callback parameter types from a method description.
    /// Looks for patterns like [code]Type[/code] or [code skip-lint]Type[/code].
    /// </summary>
    private static CallbackSignature? ExtractSignatureFromDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return null;

        var signature = new CallbackSignature();
        var parameters = new List<CallbackParameter>();

        // Pattern to match [code skip-lint]Type[/code] or [code]Type[/code]
        var codePattern = new Regex(@"\[code(?:\s+skip-lint)?\]([^\[]+)\[/code\]", RegexOptions.IgnoreCase);
        var matches = codePattern.Matches(description);

        if (matches.Count == 0)
            return null;

        // Common patterns in descriptions:
        // "The callback receives a X on success, or a Y describing the error"
        // "callback receives X"

        var receivesPattern = new Regex(@"callback\s+receives\s+(?:a\s+)?", RegexOptions.IgnoreCase);
        if (!receivesPattern.IsMatch(description))
            return null;

        int paramIndex = 0;
        foreach (Match match in matches)
        {
            var type = match.Groups[1].Value.Trim();

            // Skip if it's not a type (e.g., just a code example)
            if (type.Contains("(") || type.Contains("=") || type.Length > 50)
                continue;

            var paramName = InferParameterName(type, paramIndex);
            parameters.Add(new CallbackParameter
            {
                Name = paramName,
                Type = type
            });
            paramIndex++;
        }

        if (parameters.Count == 0)
            return null;

        signature.Parameters = parameters;
        return signature;
    }

    /// <summary>
    /// Infers a reasonable parameter name from the type.
    /// </summary>
    private static string InferParameterName(string type, int index)
    {
        // Handle Array[X] -> items, xs, etc.
        var arrayMatch = Regex.Match(type, @"Array\[(\w+)\]");
        if (arrayMatch.Success)
        {
            var innerType = arrayMatch.Groups[1].Value;
            return ToCamelCase(innerType) + "s";
        }

        // Common type to name mappings
        return type switch
        {
            "String" when index > 0 => "error",
            "String" => "result",
            "int" => "status",
            "bool" => "success",
            "Image" => "image",
            "PackedByteArray" => "data",
            _ => ToCamelCase(type)
        };
    }

    private static string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        // Handle array types
        if (name.StartsWith("Array["))
            return "items";

        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }

    /// <summary>
    /// Merges extracted signatures with existing ones, preserving manual edits.
    /// </summary>
    public static CallbackSignaturesFile Merge(CallbackSignaturesFile existing, CallbackSignaturesFile extracted)
    {
        var result = new CallbackSignaturesFile();

        // Start with existing
        foreach (var kvp in existing.Callbacks)
        {
            result.Callbacks[kvp.Key] = kvp.Value;
        }

        // Add new ones from extracted (don't overwrite existing)
        foreach (var kvp in extracted.Callbacks)
        {
            if (!result.Callbacks.ContainsKey(kvp.Key))
            {
                result.Callbacks[kvp.Key] = kvp.Value;
            }
        }

        return result;
    }
}
