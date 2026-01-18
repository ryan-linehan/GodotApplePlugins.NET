using Godot;

namespace GodotApplePlugins.Sharp.Authentication;

/// <summary>
/// C# wrapper for the ASWebAuthenticationSession GDExtension class.
/// Presents a secure web-based authentication flow (OAuth) using the system browser.
/// </summary>
public class ASWebAuthenticationSession
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Event raised upon successful authentication with the redirect URL.
    /// </summary>
    public event Action<string>? Completed;

    /// <summary>
    /// Event raised when authentication encounters an error.
    /// </summary>
    public event Action<string>? Failed;

    /// <summary>
    /// Event raised when the user dismisses the authentication interface.
    /// </summary>
    public event Action? Canceled;

    /// <summary>
    /// Creates a new ASWebAuthenticationSession wrapper.
    /// </summary>
    public ASWebAuthenticationSession(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ConnectSignals();
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Initiates the authentication session.
    /// </summary>
    /// <param name="authUrl">The authentication provider URL.</param>
    /// <param name="callbackScheme">The callback URL scheme to restrict redirects to.</param>
    /// <param name="prefersEphemeral">
    /// If true, uses an ephemeral browser session to prevent cookie sharing.
    /// </param>
    /// <returns>True if the session was successfully started.</returns>
    public bool Start(string authUrl, string callbackScheme, bool prefersEphemeral = false)
    {
        return _instance.Call(new StringName("start"), authUrl, callbackScheme, prefersEphemeral).AsBool();
    }

    /// <summary>
    /// Terminates any active authentication session.
    /// </summary>
    public void Cancel()
    {
        _instance.Call(new StringName("cancel"));
    }

    private void ConnectSignals()
    {
        _instance.Connect(new StringName("completed"),
            Callable.From<string>(callbackUrl => Completed?.Invoke(callbackUrl)));

        _instance.Connect(new StringName("failed"),
            Callable.From<string>(message => Failed?.Invoke(message)));

        _instance.Connect(new StringName("canceled"),
            Callable.From(() => Canceled?.Invoke()));
    }
}
