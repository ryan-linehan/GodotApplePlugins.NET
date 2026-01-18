using Godot;

namespace GodotApplePlugins.Sharp.Authentication;

/// <summary>
/// C# wrapper for the ASPasswordCredential GDExtension class.
/// Contains password credentials from Keychain.
/// </summary>
public class PasswordCredential
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new PasswordCredential wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The ASPasswordCredential GDExtension object.</param>
    public PasswordCredential(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The username for this credential.
    /// </summary>
    public string User => _instance.Get(new StringName("user")).AsString();

    /// <summary>
    /// The password for this credential.
    /// </summary>
    public string Password => _instance.Get(new StringName("password")).AsString();
}
