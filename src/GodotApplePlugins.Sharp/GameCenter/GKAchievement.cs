using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKAchievement GDExtension class.
/// Represents a Game Center achievement.
/// </summary>
public class GKAchievement
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKAchievement wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The GKAchievement GDExtension object.</param>
    public GKAchievement(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The achievement identifier configured on App Store Connect.
    /// </summary>
    public string Identifier
    {
        get => _instance.Get(ApplePluginStringNames.Identifier).AsString();
        set => _instance.Set(ApplePluginStringNames.Identifier, value);
    }

    /// <summary>
    /// Player's completion percentage (0-100).
    /// </summary>
    public double PercentComplete
    {
        get => _instance.Get(ApplePluginStringNames.PercentComplete).AsDouble();
        set => _instance.Set(ApplePluginStringNames.PercentComplete, value);
    }

    /// <summary>
    /// Controls whether the system displays an achievement toast at 100%.
    /// </summary>
    public bool ShowsCompletionBanner
    {
        get => _instance.Get(ApplePluginStringNames.ShowsCompletionBanner).AsBool();
        set => _instance.Set(ApplePluginStringNames.ShowsCompletionBanner, value);
    }

    /// <summary>
    /// Mirrors GameKit's completion status.
    /// </summary>
    public bool IsCompleted => _instance.Get(ApplePluginStringNames.IsCompleted).AsBool();

    /// <summary>
    /// Timestamp of last report.
    /// </summary>
    public double LastReportedDate => _instance.Get(ApplePluginStringNames.LastReportedDate).AsDouble();

    /// <summary>
    /// The achievement owner, if resolved.
    /// </summary>
    public GKPlayer? Player
    {
        get
        {
            var result = _instance.Get(ApplePluginStringNames.Player);
            return result.Obj is GodotObject obj ? new GKPlayer(obj) : null;
        }
    }

    /// <summary>
    /// Loads the achievements that the local player has already reported.
    /// </summary>
    /// <param name="achievementClass">The GKAchievement class object from the GDExtension.</param>
    /// <param name="callback">
    /// Callback invoked with the list of achievements and an optional error message.
    /// Error is null on success.
    /// </param>
    public static void LoadAchievements(GodotObject achievementClass, Action<GKAchievement[], string?> callback)
    {
        var callable = Callable.From((Godot.Collections.Array achievements, Variant error) =>
        {
            var items = achievements
                .Select(a => new GKAchievement((GodotObject)a.Obj!))
                .ToArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(items, errorMsg);
        });
        achievementClass.Call(ApplePluginStringNames.LoadAchievements, callable);
    }

    /// <summary>
    /// Submits updated achievement states.
    /// </summary>
    /// <param name="achievementClass">The GKAchievement class object from the GDExtension.</param>
    /// <param name="achievements">The achievements to report.</param>
    /// <param name="callback">
    /// Callback invoked with an optional error message. Null on success.
    /// </param>
    public static void ReportAchievements(GodotObject achievementClass, GKAchievement[] achievements, Action<string?> callback)
    {
        var godotArray = new Godot.Collections.Array();
        foreach (var achievement in achievements)
        {
            godotArray.Add(achievement.Instance);
        }

        var callable = Callable.From((Variant error) =>
        {
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(errorMsg);
        });
        achievementClass.Call(ApplePluginStringNames.ReportAchievement, godotArray, callable);
    }

    /// <summary>
    /// Asks GameKit to clear every achievement for the local player.
    /// </summary>
    /// <param name="achievementClass">The GKAchievement class object from the GDExtension.</param>
    /// <param name="callback">
    /// Callback invoked with an optional error message. Null on success.
    /// </param>
    public static void ResetAchievements(GodotObject achievementClass, Action<string?> callback)
    {
        var callable = Callable.From((Variant error) =>
        {
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(errorMsg);
        });
        achievementClass.Call(ApplePluginStringNames.ResetAchievements, callable);
    }
}
