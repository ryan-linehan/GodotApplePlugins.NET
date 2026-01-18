using System.Text.RegularExpressions;

namespace GodotApplePlugins.Generator;

/// <summary>
/// Maps Godot types to C# types.
/// </summary>
public static class TypeMapper
{
    // Classes that are wrapped by our library (not Godot built-ins)
    private static readonly HashSet<string> WrappedClasses = new()
    {
        // GameCenter
        "GameCenterManager", "GKLocalPlayer", "GKPlayer", "GKAccessPoint",
        "GKAchievement", "GKAchievementDescription", "GKLeaderboard", "GKLeaderboardEntry",
        "GKLeaderboardSet", "GKSavedGame", "GKMatch", "GKMatchRequest",
        "GKMatchmakerViewController", "GKGameCenterViewController",

        // StoreKit
        "StoreKitManager", "StoreProduct", "StoreTransaction",
        "StoreSubscriptionInfo", "StoreSubscriptionInfoStatus", "StoreSubscriptionInfoRenewalInfo",
        "StoreProductPurchaseOption", "StoreProductSubscriptionOffer", "StoreProductSubscriptionPeriod",

        // Authentication
        "ASAuthorizationController", "ASAuthorizationAppleIDCredential", "ASPasswordCredential",
        "ASWebAuthenticationSession",

        // AVFoundation
        "AVAudioSession",

        // Foundation
        "AppleURL",

        // UI
        "AppleFilePicker",

        // Other
        "SignalProxy"
    };

    /// <summary>
    /// Gets the C# type for a Godot type.
    /// </summary>
    public static string GetCSharpType(string godotType)
    {
        // Handle array types like Array[StoreProduct]
        var arrayMatch = Regex.Match(godotType, @"Array\[(\w+)\]");
        if (arrayMatch.Success)
        {
            var innerType = arrayMatch.Groups[1].Value;
            var csharpInner = GetCSharpType(innerType);
            return $"{csharpInner}[]";
        }

        // Handle typed arrays
        if (godotType.StartsWith("Array"))
        {
            return "Godot.Collections.Array";
        }

        return godotType switch
        {
            // Primitives
            "void" => "void",
            "bool" => "bool",
            "int" => "int",
            "float" => "double",
            "String" => "string",

            // Godot types
            "Variant" => "Variant",
            "Object" => "GodotObject",
            "Callable" => "Callable",
            "PackedStringArray" => "string[]",
            "PackedByteArray" => "byte[]",
            "PackedInt32Array" => "int[]",
            "PackedInt64Array" => "long[]",
            "PackedFloat32Array" => "float[]",
            "PackedFloat64Array" => "double[]",
            "Dictionary" => "Godot.Collections.Dictionary",
            "Rect2" => "Rect2",
            "Vector2" => "Vector2",
            "Vector3" => "Vector3",
            "Color" => "Color",
            "Image" => "Image",
            "Texture2D" => "Texture2D",

            // Wrapped classes get their wrapper type
            _ when WrappedClasses.Contains(godotType) => godotType,

            // Unknown types become GodotObject
            _ => "GodotObject"
        };
    }

    /// <summary>
    /// Gets the code to convert a C# value to a Godot Variant for method calls.
    /// </summary>
    public static string GetToGodotConversion(string paramName, string godotType)
    {
        var arrayMatch = Regex.Match(godotType, @"Array\[(\w+)\]");
        if (arrayMatch.Success)
        {
            var innerType = arrayMatch.Groups[1].Value;
            if (WrappedClasses.Contains(innerType))
            {
                // Need to unwrap each element and convert to Variant
                return $"new Godot.Collections.Array({paramName}.Select(x => Variant.From(x.Instance)))";
            }
            // Convert primitive arrays to Variant enumerable
            return $"new Godot.Collections.Array({paramName}.Select(x => Variant.From(x)))";
        }

        // Handle PackedStringArray - pass directly, Godot handles the conversion
        if (godotType == "PackedStringArray")
        {
            return paramName;
        }

        if (WrappedClasses.Contains(godotType))
        {
            return $"{paramName}.Instance";
        }

        return paramName;
    }

    /// <summary>
    /// Gets the code to convert a Godot Variant to a C# value.
    /// </summary>
    public static string GetFromGodotConversion(string variantExpr, string godotType)
    {
        var arrayMatch = Regex.Match(godotType, @"Array\[(\w+)\]");
        if (arrayMatch.Success)
        {
            var innerType = arrayMatch.Groups[1].Value;
            if (WrappedClasses.Contains(innerType))
            {
                return $"{variantExpr}.AsGodotArray().Select(x => new {innerType}((GodotObject)x.Obj!)).ToArray()";
            }
            var csharpInner = GetCSharpType(innerType);
            return $"{variantExpr}.AsGodotArray().Select(x => x.As<{csharpInner}>()).ToArray()";
        }

        if (WrappedClasses.Contains(godotType))
        {
            return $"new {godotType}((GodotObject){variantExpr}.Obj!)";
        }

        return godotType switch
        {
            "void" => "",
            "bool" => $"{variantExpr}.AsBool()",
            "int" => $"{variantExpr}.AsInt32()",
            "float" => $"{variantExpr}.AsDouble()",
            "String" => $"{variantExpr}.AsString()",
            "PackedStringArray" => $"{variantExpr}.AsStringArray()",
            "PackedByteArray" => $"{variantExpr}.AsByteArray()",
            "Rect2" => $"{variantExpr}.AsRect2()",
            "Vector2" => $"{variantExpr}.AsVector2()",
            "Vector3" => $"{variantExpr}.AsVector3()",
            "Color" => $"{variantExpr}.AsColor()",
            "Dictionary" => $"{variantExpr}.AsGodotDictionary()",
            "Array" => $"{variantExpr}.AsGodotArray()",
            "Image" => $"{variantExpr}.As<Image>()",
            "Texture2D" => $"{variantExpr}.As<Texture2D>()",
            "Object" => $"{variantExpr}.AsGodotObject()",
            "Callable" => $"{variantExpr}.AsCallable()",
            "Variant" => variantExpr,
            _ => $"{variantExpr}.AsGodotObject()"
        };
    }

    /// <summary>
    /// Checks if the type is a wrapped class.
    /// </summary>
    public static bool IsWrappedClass(string godotType) => WrappedClasses.Contains(godotType);

    /// <summary>
    /// Converts snake_case to PascalCase.
    /// </summary>
    public static string ToPascalCase(string snakeCase)
    {
        if (string.IsNullOrEmpty(snakeCase))
            return snakeCase;

        return string.Join("", snakeCase.Split('_')
            .Select(word => char.ToUpper(word[0]) + word[1..].ToLower()));
    }

    /// <summary>
    /// Converts snake_case to camelCase.
    /// </summary>
    public static string ToCamelCase(string snakeCase)
    {
        var pascal = ToPascalCase(snakeCase);
        if (string.IsNullOrEmpty(pascal))
            return pascal;
        return char.ToLower(pascal[0]) + pascal[1..];
    }

    /// <summary>
    /// Gets the namespace for a class based on its name.
    /// </summary>
    public static string GetNamespace(string className)
    {
        return className switch
        {
            _ when className.StartsWith("GK") || className == "GameCenterManager"
                => "GodotApplePlugins.NET.GameCenter",
            _ when className.StartsWith("Store") || className == "ProductView"
                   || className.Contains("Subscription")
                => "GodotApplePlugins.NET.StoreKit",
            _ when className.StartsWith("AS")
                => "GodotApplePlugins.NET.Authentication",
            _ when className.StartsWith("AV")
                => "GodotApplePlugins.NET.AVFoundation",
            _ when className == "Foundation" || className == "AppleURL"
                => "GodotApplePlugins.NET.Foundation",
            _ when className == "AppleFilePicker"
                => "GodotApplePlugins.NET.UI",
            _ => "GodotApplePlugins.NET"
        };
    }
}
