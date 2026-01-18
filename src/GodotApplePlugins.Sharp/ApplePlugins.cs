using Godot;
using GodotApplePlugins.Sharp.Authentication;
using GodotApplePlugins.Sharp.GameCenter;
using GodotApplePlugins.Sharp.StoreKit;

namespace GodotApplePlugins.Sharp;

/// <summary>
/// Main entry point for accessing GodotApplePlugins functionality.
/// Provides convenient factory methods for creating wrapper instances.
/// </summary>
public static class ApplePlugins
{
    /// <summary>
    /// Creates a GameCenterManager wrapper from a GDExtension instance.
    /// </summary>
    /// <param name="instance">The GameCenterManager GDExtension object.</param>
    /// <returns>A new GameCenterManager wrapper.</returns>
    public static GameCenterManager CreateGameCenterManager(GodotObject instance)
    {
        return new GameCenterManager(instance);
    }

    /// <summary>
    /// Creates a StoreKitManager wrapper from a GDExtension instance.
    /// </summary>
    /// <param name="instance">The StoreKitManager GDExtension object.</param>
    /// <returns>A new StoreKitManager wrapper.</returns>
    public static StoreKitManager CreateStoreKitManager(GodotObject instance)
    {
        return new StoreKitManager(instance);
    }

    /// <summary>
    /// Creates an ASAuthorizationController wrapper from a GDExtension instance.
    /// </summary>
    /// <param name="instance">The ASAuthorizationController GDExtension object.</param>
    /// <returns>A new ASAuthorizationController wrapper.</returns>
    public static ASAuthorizationController CreateAuthorizationController(GodotObject instance)
    {
        return new ASAuthorizationController(instance);
    }

    /// <summary>
    /// Checks if the GodotApplePlugins GDExtension is available on this platform.
    /// </summary>
    /// <returns>True if running on macOS or iOS with the plugin loaded.</returns>
    public static bool IsAvailable()
    {
        var os = OS.GetName();
        return os == "macOS" || os == "iOS";
    }

    /// <summary>
    /// Tries to create a new instance of a GDExtension class by name.
    /// </summary>
    /// <param name="className">The GDExtension class name (e.g., "GameCenterManager").</param>
    /// <returns>The new instance, or null if the class is not available.</returns>
    public static GodotObject? TryCreateInstance(string className)
    {
        if (!ClassDB.ClassExists(className))
            return null;

        return ClassDB.Instantiate(className).AsGodotObject();
    }

    /// <summary>
    /// Creates a new GameCenterManager instance if available.
    /// </summary>
    /// <returns>A new GameCenterManager wrapper, or null if not available.</returns>
    public static GameCenterManager? TryCreateGameCenterManager()
    {
        var instance = TryCreateInstance("GameCenterManager");
        return instance != null ? new GameCenterManager(instance) : null;
    }

    /// <summary>
    /// Creates a new StoreKitManager instance if available.
    /// </summary>
    /// <returns>A new StoreKitManager wrapper, or null if not available.</returns>
    public static StoreKitManager? TryCreateStoreKitManager()
    {
        var instance = TryCreateInstance("StoreKitManager");
        return instance != null ? new StoreKitManager(instance) : null;
    }

    /// <summary>
    /// Creates a new ASAuthorizationController instance if available.
    /// </summary>
    /// <returns>A new ASAuthorizationController wrapper, or null if not available.</returns>
    public static ASAuthorizationController? TryCreateAuthorizationController()
    {
        var instance = TryCreateInstance("ASAuthorizationController");
        return instance != null ? new ASAuthorizationController(instance) : null;
    }
}
