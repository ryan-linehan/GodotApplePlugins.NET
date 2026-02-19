#nullable enable

using Godot;
using Godot.Collections;

namespace GodotPlayGameServices.NET;

/// <summary>
/// Internal helpers for safely extracting values from Godot dictionaries.
/// </summary>
internal static class DictionaryExtensions
{
    public static string GetStringOrDefault(this Dictionary dict, string key, string defaultValue = "")
    {
        if (dict.ContainsKey(key))
            return dict[key].AsString();
        return defaultValue;
    }

    public static int GetIntOrDefault(this Dictionary dict, string key, int defaultValue = 0)
    {
        if (dict.ContainsKey(key))
            return dict[key].AsInt32();
        return defaultValue;
    }

    public static long GetLongOrDefault(this Dictionary dict, string key, long defaultValue = 0)
    {
        if (dict.ContainsKey(key))
            return dict[key].AsInt64();
        return defaultValue;
    }

    public static bool GetBoolOrDefault(this Dictionary dict, string key, bool defaultValue = false)
    {
        if (dict.ContainsKey(key))
            return dict[key].AsBool();
        return defaultValue;
    }

    public static Dictionary? GetDictionaryOrNull(this Dictionary dict, string key)
    {
        if (dict.ContainsKey(key))
        {
            var value = dict[key];
            if (value.VariantType == Variant.Type.Dictionary)
                return value.AsGodotDictionary();
        }
        return null;
    }
}
