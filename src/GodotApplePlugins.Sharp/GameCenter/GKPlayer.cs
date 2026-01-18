using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKPlayer GDExtension class.
/// Represents a Game Center player.
/// </summary>
public class GKPlayer
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKPlayer wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The GKPlayer GDExtension object.</param>
    public GKPlayer(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The player's game-scoped identifier.
    /// </summary>
    public string GamePlayerId => _instance.Get(ApplePluginStringNames.GamePlayerId).AsString();

    /// <summary>
    /// The player's team-scoped identifier.
    /// </summary>
    public string TeamPlayerId => _instance.Get(ApplePluginStringNames.TeamPlayerId).AsString();

    /// <summary>
    /// The player's display name.
    /// </summary>
    public string DisplayName => _instance.Get(ApplePluginStringNames.DisplayName).AsString();

    /// <summary>
    /// The player's alias.
    /// </summary>
    public string Alias => _instance.Get(ApplePluginStringNames.Alias).AsString();

    /// <summary>
    /// Indicates if this player was invited to the game.
    /// </summary>
    public bool IsInvitedToGame => _instance.Get(ApplePluginStringNames.IsInvitedToGame).AsBool();

    /// <summary>
    /// Loads the player's photo.
    /// </summary>
    /// <param name="size">The photo size to load.</param>
    /// <param name="callback">
    /// Callback invoked with the photo image and an optional error message.
    /// </param>
    public void LoadPhoto(GKPlayerPhotoSize size, Action<Image?, string?> callback)
    {
        var callable = Callable.From((Variant image, Variant error) =>
        {
            Image? photo = null;
            if (image.Obj is Image img)
            {
                photo = img;
            }
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(photo, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.LoadPhoto, (int)size, callable);
    }
}
