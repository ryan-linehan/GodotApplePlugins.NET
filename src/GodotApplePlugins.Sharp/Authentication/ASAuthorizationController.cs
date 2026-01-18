using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.Authentication;

/// <summary>
/// C# wrapper for the ASAuthorizationController GDExtension class.
/// Manages authorization requests for Apple ID and other services (Sign in with Apple).
/// </summary>
public class ASAuthorizationController
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Event raised when authorization completes successfully.
    /// </summary>
    public event Action<AppleIdCredential>? AuthorizationCompleted;

    /// <summary>
    /// Event raised when authorization completes with a password credential.
    /// </summary>
    public event Action<PasswordCredential>? PasswordCredentialReceived;

    /// <summary>
    /// Event raised when authorization fails.
    /// </summary>
    public event Action<string>? AuthorizationFailed;

    /// <summary>
    /// Creates a new ASAuthorizationController wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The ASAuthorizationController GDExtension object.</param>
    public ASAuthorizationController(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ConnectSignals();
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Initiates the default Sign in with Apple workflow with email and full_name scopes.
    /// Listen to <see cref="AuthorizationCompleted"/> and <see cref="AuthorizationFailed"/> for results.
    /// </summary>
    public void SignIn()
    {
        _instance.Call(ApplePluginStringNames.Signin);
    }

    /// <summary>
    /// Starts Sign in with Apple with custom scopes.
    /// </summary>
    /// <param name="scopes">Array of scope strings (use <see cref="ASAuthorizationScopes"/>).</param>
    public void SignInWithScopes(string[] scopes)
    {
        var scopeArray = new Godot.Collections.Array();
        foreach (var scope in scopes)
        {
            scopeArray.Add(scope);
        }
        _instance.Call(ApplePluginStringNames.SigninWithScopes, scopeArray);
    }

    /// <summary>
    /// Signs in with Apple requesting only email access.
    /// </summary>
    public void SignInWithEmail()
    {
        SignInWithScopes(new[] { ASAuthorizationScopes.Email });
    }

    /// <summary>
    /// Signs in with Apple requesting only full name access.
    /// </summary>
    public void SignInWithFullName()
    {
        SignInWithScopes(new[] { ASAuthorizationScopes.FullName });
    }

    /// <summary>
    /// Signs in with Apple requesting both email and full name access.
    /// </summary>
    public void SignInWithEmailAndFullName()
    {
        SignInWithScopes(new[] { ASAuthorizationScopes.Email, ASAuthorizationScopes.FullName });
    }

    private void ConnectSignals()
    {
        _instance.Connect(ApplePluginStringNames.AuthorizationCompletedSignal,
            Callable.From<GodotObject>(OnAuthorizationCompleted));

        _instance.Connect(ApplePluginStringNames.AuthorizationFailedSignal,
            Callable.From<string>(OnAuthorizationFailed));
    }

    private void OnAuthorizationCompleted(GodotObject credential)
    {
        // Check the type of credential and dispatch appropriate event
        var className = credential.GetClass();

        if (className.Contains("AppleID") || className.Contains("ASAuthorizationAppleIDCredential"))
        {
            var appleIdCredential = new AppleIdCredential(credential);
            AuthorizationCompleted?.Invoke(appleIdCredential);
        }
        else if (className.Contains("Password") || className.Contains("ASPasswordCredential"))
        {
            var passwordCredential = new PasswordCredential(credential);
            PasswordCredentialReceived?.Invoke(passwordCredential);
        }
        else
        {
            // Default to Apple ID credential
            var appleIdCredential = new AppleIdCredential(credential);
            AuthorizationCompleted?.Invoke(appleIdCredential);
        }
    }

    private void OnAuthorizationFailed(string message)
    {
        AuthorizationFailed?.Invoke(message);
    }

    /// <summary>
    /// Disconnects signal handlers. Call this when disposing of the controller.
    /// </summary>
    public void Disconnect()
    {
        // Note: In practice, you may want to store the Callables for proper disconnection
    }
}
