using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GameCenterManager GDExtension class.
/// Entry point for authenticating the Apple Game Center local player.
/// </summary>
public class GameCenterManager
{
    private readonly GodotObject _instance;
    private GKLocalPlayer? _localPlayer;
    private GKAccessPoint? _accessPoint;

    /// <summary>
    /// Event raised when Game Center authentication succeeds or fails.
    /// </summary>
    public event Action<bool>? AuthenticationResult;

    /// <summary>
    /// Event raised when Game Center authentication fails with an error message.
    /// </summary>
    public event Action<string>? AuthenticationError;

    /// <summary>
    /// Creates a new GameCenterManager wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The GameCenterManager GDExtension object.</param>
    public GameCenterManager(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ConnectSignals();
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Gets the local player wrapper. Available after successful authentication.
    /// </summary>
    public GKLocalPlayer? LocalPlayer
    {
        get
        {
            if (_localPlayer != null) return _localPlayer;

            var result = _instance.Call(ApplePluginStringNames.GetLocalPlayer);
            if (result.Obj is GodotObject obj)
            {
                _localPlayer = new GKLocalPlayer(obj);
            }
            return _localPlayer;
        }
    }

    /// <summary>
    /// Gets the Game Center access point for displaying the Game Center UI.
    /// </summary>
    public GKAccessPoint? AccessPoint
    {
        get
        {
            if (_accessPoint != null) return _accessPoint;

            var result = _instance.Call(ApplePluginStringNames.GetAccessPoint);
            if (result.Obj is GodotObject obj)
            {
                _accessPoint = new GKAccessPoint(obj);
            }
            return _accessPoint;
        }
    }

    /// <summary>
    /// Triggers GKLocalPlayer.authenticateHandler, presenting Apple's login sheet when needed.
    /// Listen to <see cref="AuthenticationResult"/> and <see cref="AuthenticationError"/> for results.
    /// </summary>
    public void Authenticate()
    {
        _instance.Call(ApplePluginStringNames.Authenticate);
    }

    private void ConnectSignals()
    {
        _instance.Connect(ApplePluginStringNames.AuthenticationResultSignal,
            Callable.From<bool>(OnAuthenticationResult));
        _instance.Connect(ApplePluginStringNames.AuthenticationErrorSignal,
            Callable.From<string>(OnAuthenticationError));
    }

    private void OnAuthenticationResult(bool status)
    {
        // Clear cached local player on auth change so it gets refreshed
        _localPlayer = null;
        AuthenticationResult?.Invoke(status);
    }

    private void OnAuthenticationError(string message)
    {
        AuthenticationError?.Invoke(message);
    }

    /// <summary>
    /// Disconnects signal handlers. Call this when disposing of the manager.
    /// </summary>
    public void Disconnect()
    {
        if (_instance.IsConnected(ApplePluginStringNames.AuthenticationResultSignal,
                Callable.From<bool>(OnAuthenticationResult)))
        {
            _instance.Disconnect(ApplePluginStringNames.AuthenticationResultSignal,
                Callable.From<bool>(OnAuthenticationResult));
        }

        if (_instance.IsConnected(ApplePluginStringNames.AuthenticationErrorSignal,
                Callable.From<string>(OnAuthenticationError)))
        {
            _instance.Disconnect(ApplePluginStringNames.AuthenticationErrorSignal,
                Callable.From<string>(OnAuthenticationError));
        }
    }
}
