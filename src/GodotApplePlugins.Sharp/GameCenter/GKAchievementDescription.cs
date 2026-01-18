using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKAchievementDescription GDExtension class.
/// Provides metadata about achievements configured in App Store Connect.
/// </summary>
public class GKAchievementDescription
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKAchievementDescription wrapper.
    /// </summary>
    public GKAchievementDescription(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Unique achievement identifier.
    /// </summary>
    public string Identifier => _instance.Get(new StringName("identifier")).AsString();

    /// <summary>
    /// Display title, localized according to the player's language.
    /// </summary>
    public string Title => _instance.Get(new StringName("title")).AsString();

    /// <summary>
    /// Text displayed after achievement completion.
    /// </summary>
    public string AchievedDescription => _instance.Get(new StringName("achieved_description")).AsString();

    /// <summary>
    /// Text displayed before achievement completion.
    /// </summary>
    public string UnachievedDescription => _instance.Get(new StringName("unachieved_description")).AsString();

    /// <summary>
    /// Point value when completed.
    /// </summary>
    public int MaximumPoints => _instance.Get(new StringName("maximum_points")).AsInt32();

    /// <summary>
    /// Optional grouping from App Store Connect.
    /// </summary>
    public string GroupIdentifier => _instance.Get(new StringName("group_identifier")).AsString();

    /// <summary>
    /// Whether the achievement is hidden from the player.
    /// </summary>
    public bool IsHidden => _instance.Get(new StringName("is_hidden")).AsBool();

    /// <summary>
    /// Whether the achievement can be earned multiple times.
    /// </summary>
    public bool IsReplayable => _instance.Get(new StringName("is_replayable")).AsBool();

    /// <summary>
    /// Apple's reported rarity percentage, or null if unavailable.
    /// </summary>
    public double? RarityPercent
    {
        get
        {
            var result = _instance.Get(new StringName("rarity_percent"));
            if (result.VariantType == Variant.Type.Nil)
                return null;
            return result.AsDouble();
        }
    }

    /// <summary>
    /// Loads the entire catalog of achievement descriptions.
    /// </summary>
    /// <param name="callback">Callback with array of descriptions and optional error message.</param>
    public static void LoadAchievementDescriptions(GodotObject classInstance, Action<GKAchievementDescription[], string?> callback)
    {
        var callable = Callable.From((Godot.Collections.Array descriptions, Variant error) =>
        {
            var wrapped = descriptions
                .Select(d => new GKAchievementDescription((GodotObject)d.Obj!))
                .ToArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(wrapped, errorMsg);
        });
        classInstance.Call(new StringName("load_achievement_descriptions"), callable);
    }

    /// <summary>
    /// Downloads the achievement icon image.
    /// </summary>
    /// <param name="callback">Callback with the image and optional error message.</param>
    public void LoadImage(Action<Texture2D?, string?> callback)
    {
        var callable = Callable.From((Variant image, Variant error) =>
        {
            var texture = image.Obj as Texture2D;
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(texture, errorMsg);
        });
        _instance.Call(new StringName("load_image"), callable);
    }
}
