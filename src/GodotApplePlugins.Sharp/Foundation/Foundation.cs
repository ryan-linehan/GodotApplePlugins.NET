using Godot;

namespace GodotApplePlugins.Sharp.Foundation;

/// <summary>
/// C# wrapper for the Foundation GDExtension class.
/// Exposes common APIs from Apple's Foundation framework.
/// </summary>
public static class AppleFoundation
{
    private static readonly StringName UuidMethod = new("uuid");

    /// <summary>
    /// Generates a Foundation UUID and returns its string representation.
    /// </summary>
    /// <param name="classInstance">The Foundation class instance from the GDExtension.</param>
    /// <returns>A new UUID string.</returns>
    public static string GenerateUuid(GodotObject classInstance)
    {
        return classInstance.Call(UuidMethod).AsString();
    }
}
