using Godot;
using GodotApplePlugins.Sharp.Foundation;

namespace GodotApplePlugins.Sharp.UI;

/// <summary>
/// C# wrapper for the AppleFilePicker GDExtension class.
/// Provides native file picker access on iOS and macOS platforms.
/// </summary>
public class AppleFilePicker
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Event raised when the user dismisses the picker without selecting files.
    /// </summary>
    public event Action? Canceled;

    /// <summary>
    /// Event raised upon selecting a single file (when allowMultiple is false).
    /// </summary>
    public event Action<AppleURL, string>? FileSelected;

    /// <summary>
    /// Event raised upon selecting multiple files (when allowMultiple is true).
    /// </summary>
    public event Action<AppleURL[], string[]>? FilesSelected;

    /// <summary>
    /// Creates a new AppleFilePicker wrapper.
    /// </summary>
    public AppleFilePicker(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ConnectSignals();
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Opens the system file picker dialog.
    /// </summary>
    /// <param name="allowedTypes">
    /// File extensions (e.g., "txt", "png") or UTTypes (e.g., "public.plain-text").
    /// </param>
    /// <param name="allowMultiple">
    /// If true, users can select multiple files and results emit via FilesSelected;
    /// otherwise results emit via FileSelected.
    /// </param>
    public void PickDocument(string[] allowedTypes, bool allowMultiple = false)
    {
        var typesArray = new Godot.Collections.Array<string>();
        foreach (var type in allowedTypes)
        {
            typesArray.Add(type);
        }
        _instance.Call(new StringName("pick_document"), typesArray, allowMultiple);
    }

    private void ConnectSignals()
    {
        _instance.Connect(new StringName("canceled"),
            Callable.From(() => Canceled?.Invoke()));

        _instance.Connect(new StringName("file_selected"),
            Callable.From<GodotObject, string>((url, path) =>
                FileSelected?.Invoke(new AppleURL(url), path)));

        _instance.Connect(new StringName("files_selected"),
            Callable.From<Godot.Collections.Array, string[]>((urls, paths) =>
            {
                var wrappedUrls = urls
                    .Select(u => new AppleURL((GodotObject)u.Obj!))
                    .ToArray();
                FilesSelected?.Invoke(wrappedUrls, paths);
            }));
    }
}
