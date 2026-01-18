using Godot;
using GodotApplePlugins.Sharp.Authentication;
using GodotApplePlugins.Sharp.AVFoundation;
using GodotApplePlugins.Sharp.Foundation;
using GodotApplePlugins.Sharp.GameCenter;
using GodotApplePlugins.Sharp.StoreKit;
using GodotApplePlugins.Sharp.UI;

namespace GodotApplePlugins.Sharp;

/// <summary>
/// Main entry point for accessing GodotApplePlugins functionality.
/// Provides convenient factory methods for creating wrapper instances.
/// </summary>
public static class ApplePlugins
{
    #region Platform Check

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
    /// Gets a class reference for calling static methods.
    /// </summary>
    /// <param name="className">The class name.</param>
    /// <returns>The class object, or null if not available.</returns>
    public static GodotObject? GetClass(string className)
    {
        if (!ClassDB.ClassExists(className))
            return null;

        // For static methods, we need to instantiate and use the instance
        return ClassDB.Instantiate(className).AsGodotObject();
    }

    #endregion

    #region GameCenter

    /// <summary>
    /// Creates a GameCenterManager wrapper from a GDExtension instance.
    /// </summary>
    public static GameCenterManager CreateGameCenterManager(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new GameCenterManager instance if available.
    /// </summary>
    public static GameCenterManager? TryCreateGameCenterManager()
    {
        var instance = TryCreateInstance("GameCenterManager");
        return instance != null ? new GameCenterManager(instance) : null;
    }

    /// <summary>
    /// Creates a GKMatchRequest wrapper from a GDExtension instance.
    /// </summary>
    public static GKMatchRequest CreateMatchRequest(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new GKMatchRequest instance if available.
    /// </summary>
    public static GKMatchRequest? TryCreateMatchRequest()
    {
        var instance = TryCreateInstance("GKMatchRequest");
        return instance != null ? new GKMatchRequest(instance) : null;
    }

    /// <summary>
    /// Creates a GKMatchmakerViewController wrapper from a GDExtension instance.
    /// </summary>
    public static GKMatchmakerViewController CreateMatchmakerViewController(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new GKMatchmakerViewController instance if available.
    /// </summary>
    public static GKMatchmakerViewController? TryCreateMatchmakerViewController()
    {
        var instance = TryCreateInstance("GKMatchmakerViewController");
        return instance != null ? new GKMatchmakerViewController(instance) : null;
    }

    #endregion

    #region StoreKit

    /// <summary>
    /// Creates a StoreKitManager wrapper from a GDExtension instance.
    /// </summary>
    public static StoreKitManager CreateStoreKitManager(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new StoreKitManager instance if available.
    /// </summary>
    public static StoreKitManager? TryCreateStoreKitManager()
    {
        var instance = TryCreateInstance("StoreKitManager");
        return instance != null ? new StoreKitManager(instance) : null;
    }

    #endregion

    #region Authentication

    /// <summary>
    /// Creates an ASAuthorizationController wrapper from a GDExtension instance.
    /// </summary>
    public static ASAuthorizationController CreateAuthorizationController(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new ASAuthorizationController instance if available.
    /// </summary>
    public static ASAuthorizationController? TryCreateAuthorizationController()
    {
        var instance = TryCreateInstance("ASAuthorizationController");
        return instance != null ? new ASAuthorizationController(instance) : null;
    }

    /// <summary>
    /// Creates an ASWebAuthenticationSession wrapper from a GDExtension instance.
    /// </summary>
    public static ASWebAuthenticationSession CreateWebAuthenticationSession(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new ASWebAuthenticationSession instance if available.
    /// </summary>
    public static ASWebAuthenticationSession? TryCreateWebAuthenticationSession()
    {
        var instance = TryCreateInstance("ASWebAuthenticationSession");
        return instance != null ? new ASWebAuthenticationSession(instance) : null;
    }

    #endregion

    #region AVFoundation

    /// <summary>
    /// Creates an AVAudioSession wrapper from a GDExtension instance.
    /// </summary>
    public static AVAudioSession CreateAudioSession(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new AVAudioSession instance if available.
    /// </summary>
    public static AVAudioSession? TryCreateAudioSession()
    {
        var instance = TryCreateInstance("AVAudioSession");
        return instance != null ? new AVAudioSession(instance) : null;
    }

    #endregion

    #region Foundation

    /// <summary>
    /// Creates an AppleURL wrapper from a GDExtension instance.
    /// </summary>
    public static AppleURL CreateAppleURL(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new AppleURL instance if available.
    /// </summary>
    public static AppleURL? TryCreateAppleURL()
    {
        var instance = TryCreateInstance("AppleURL");
        return instance != null ? new AppleURL(instance) : null;
    }

    /// <summary>
    /// Generates a new UUID using Apple's Foundation framework.
    /// </summary>
    /// <returns>A new UUID string, or null if Foundation is not available.</returns>
    public static string? GenerateUuid()
    {
        var foundation = TryCreateInstance("Foundation");
        return foundation != null ? AppleFoundation.GenerateUuid(foundation) : null;
    }

    #endregion

    #region UI

    /// <summary>
    /// Creates an AppleFilePicker wrapper from a GDExtension instance.
    /// </summary>
    public static AppleFilePicker CreateFilePicker(GodotObject instance)
        => new(instance);

    /// <summary>
    /// Creates a new AppleFilePicker instance if available.
    /// </summary>
    public static AppleFilePicker? TryCreateFilePicker()
    {
        var instance = TryCreateInstance("AppleFilePicker");
        return instance != null ? new AppleFilePicker(instance) : null;
    }

    #endregion
}
