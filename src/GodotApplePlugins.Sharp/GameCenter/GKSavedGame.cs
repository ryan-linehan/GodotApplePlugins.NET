using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKSavedGame GDExtension class.
/// Represents a saved game in Game Center.
/// </summary>
public class GKSavedGame
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKSavedGame wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The GKSavedGame GDExtension object.</param>
    public GKSavedGame(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The name of the saved game.
    /// </summary>
    public string Name => _instance.Get(new StringName("name")).AsString();

    /// <summary>
    /// The device name where this save was created.
    /// </summary>
    public string DeviceName => _instance.Get(new StringName("device_name")).AsString();

    /// <summary>
    /// The modification date (Unix timestamp).
    /// </summary>
    public double ModificationDate => _instance.Get(new StringName("modification_date")).AsDouble();

    /// <summary>
    /// Loads the saved game data.
    /// </summary>
    /// <param name="callback">
    /// Callback invoked with the data bytes and an optional error message.
    /// </param>
    public void LoadData(Action<byte[], string?> callback)
    {
        var callable = Callable.From((Variant data, Variant error) =>
        {
            var bytes = data.VariantType == Variant.Type.Nil
                ? Array.Empty<byte>()
                : data.AsByteArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(bytes, errorMsg);
        });
        _instance.Call(new StringName("load_data"), callable);
    }
}
