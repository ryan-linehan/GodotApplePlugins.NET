using Godot;

namespace GodotApplePlugins.Sharp.Foundation;

/// <summary>
/// C# wrapper for the AppleURL GDExtension class.
/// Represents an Apple Foundation.URL type for file access outside the application sandbox.
/// </summary>
public class AppleURL
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new AppleURL wrapper.
    /// </summary>
    public AppleURL(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Returns the URL's absolute string representation.
    /// </summary>
    public string AbsoluteString => _instance.Call(new StringName("get_absolute_string")).AsString();

    /// <summary>
    /// Returns unencoded URL contents.
    /// </summary>
    public string Path => _instance.Call(new StringName("get_path")).AsString();

    /// <summary>
    /// Returns percent-encoded URL contents.
    /// </summary>
    public string PathEncoded => _instance.Call(new StringName("get_path_encoded")).AsString();

    /// <summary>
    /// Loads file contents as a byte array.
    /// </summary>
    public byte[] GetData()
    {
        return _instance.Call(new StringName("get_data")).AsByteArray();
    }

    /// <summary>
    /// Fetches file contents as a string.
    /// </summary>
    public string GetString()
    {
        return _instance.Call(new StringName("get_string")).AsString();
    }

    /// <summary>
    /// Parses and sets URL from a string.
    /// </summary>
    /// <param name="urlString">The URL string to parse.</param>
    /// <returns>True if the URL was successfully parsed.</returns>
    public bool SetValue(string urlString)
    {
        return _instance.Call(new StringName("set_value"), urlString).AsBool();
    }

    /// <summary>
    /// Sets URL from a file path string.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    public void SetFromFilePath(string filePath)
    {
        _instance.Call(new StringName("set_from_filepath"), filePath);
    }

    /// <summary>
    /// Makes the resource pointed to by a security-scoped URL available to the app.
    /// You must call this before accessing sandboxed files.
    /// </summary>
    /// <returns>True if access was granted.</returns>
    public bool StartAccessingSecurityScopedResource()
    {
        return _instance.Call(new StringName("start_accessing_security_scoped_resource")).AsBool();
    }

    /// <summary>
    /// Revokes access to sandboxed resources.
    /// Call this when finished accessing the resource.
    /// </summary>
    public void StopAccessingSecurityScopedResource()
    {
        _instance.Call(new StringName("stop_accessing_security_scoped_resource"));
    }
}
