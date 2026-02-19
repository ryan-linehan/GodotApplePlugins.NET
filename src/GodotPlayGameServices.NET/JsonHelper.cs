#nullable enable

using Godot;
using Godot.Collections;

namespace GodotPlayGameServices.NET;

/// <summary>
/// Internal helper for safely parsing JSON from the Android plugin.
/// </summary>
internal static class JsonHelper
{
    /// <summary>
    /// Parses a JSON string into an array of dictionaries.
    /// </summary>
    public static Dictionary[] ParseArray(string json)
    {
        var parser = new Json();
        var error = parser.Parse(json);
        if (error != Error.Ok)
        {
            GD.PrintErr($"GodotPlayGameServices.NET: Failed to parse JSON array: {parser.GetErrorMessage()}");
            return [];
        }

        var result = parser.Data;
        if (result.VariantType != Variant.Type.Array)
        {
            GD.PrintErr($"GodotPlayGameServices.NET: Expected JSON array but got {result.VariantType}");
            return [];
        }

        var array = result.AsGodotArray();
        var dictionaries = new Dictionary[array.Count];
        for (int i = 0; i < array.Count; i++)
        {
            dictionaries[i] = array[i].AsGodotDictionary();
        }
        return dictionaries;
    }

    /// <summary>
    /// Parses a JSON string into a single dictionary.
    /// </summary>
    public static Dictionary? ParseDictionary(string json)
    {
        if (string.IsNullOrEmpty(json))
            return null;

        var parser = new Json();
        var error = parser.Parse(json);
        if (error != Error.Ok)
        {
            GD.PrintErr($"GodotPlayGameServices.NET: Failed to parse JSON: {parser.GetErrorMessage()}");
            return null;
        }

        var result = parser.Data;
        if (result.VariantType == Variant.Type.Nil)
            return null;

        if (result.VariantType != Variant.Type.Dictionary)
        {
            GD.PrintErr($"GodotPlayGameServices.NET: Expected JSON object but got {result.VariantType}");
            return null;
        }

        return result.AsGodotDictionary();
    }
}
