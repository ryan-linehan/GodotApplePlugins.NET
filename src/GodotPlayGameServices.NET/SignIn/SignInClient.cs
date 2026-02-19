#nullable enable

using Godot;
using System.Threading.Tasks;

namespace GodotPlayGameServices.NET.SignIn;

/// <summary>
/// Client for Google Play Games sign-in functionality.
/// </summary>
public class SignInClient
{
    private readonly GodotObject _plugin;

    /// <summary>
    /// Emitted after checking the player's authentication status.
    /// </summary>
    public event Action<bool>? UserAuthenticated;

    /// <summary>
    /// Emitted after requesting server-side access with an OAuth authorization code.
    /// </summary>
    public event Action<string>? ServerSideAccessRequested;

    internal SignInClient(GodotObject plugin)
    {
        _plugin = plugin;

        _plugin.Connect("userAuthenticated",
            Callable.From<bool>(isAuthenticated =>
                UserAuthenticated?.Invoke(isAuthenticated)));

        _plugin.Connect("serverSideAccessRequested",
            Callable.From<string>(token =>
                ServerSideAccessRequested?.Invoke(token)));
    }

    /// <summary>
    /// Checks if the player is currently authenticated.
    /// </summary>
    public void IsAuthenticated()
    {
        _plugin.Call("isAuthenticated");
    }

    /// <summary>
    /// Initiates the sign-in flow.
    /// </summary>
    public void SignIn()
    {
        _plugin.Call("signIn");
    }

    /// <summary>
    /// Requests server-side access for backend authentication.
    /// </summary>
    /// <param name="serverClientId">The OAuth 2.0 client ID for your server.</param>
    /// <param name="forceRefreshToken">Whether to force a new refresh token.</param>
    public void RequestServerSideAccess(string serverClientId, bool forceRefreshToken)
    {
        _plugin.Call("requestServerSideAccess", serverClientId, forceRefreshToken);
    }

    /// <summary>
    /// Checks authentication status and returns the result asynchronously.
    /// </summary>
    public async Task<bool> IsAuthenticatedAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        void Handler(bool isAuth) { tcs.TrySetResult(isAuth); UserAuthenticated -= Handler; }
        UserAuthenticated += Handler;
        IsAuthenticated();
        return await tcs.Task;
    }

    /// <summary>
    /// Signs in and returns the result asynchronously.
    /// </summary>
    public async Task<bool> SignInAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        void Handler(bool isAuth) { tcs.TrySetResult(isAuth); UserAuthenticated -= Handler; }
        UserAuthenticated += Handler;
        SignIn();
        return await tcs.Task;
    }

    /// <summary>
    /// Requests server-side access and returns the authorization code asynchronously.
    /// </summary>
    public async Task<string> RequestServerSideAccessAsync(string serverClientId, bool forceRefreshToken)
    {
        var tcs = new TaskCompletionSource<string>();
        void Handler(string token) { tcs.TrySetResult(token); ServerSideAccessRequested -= Handler; }
        ServerSideAccessRequested += Handler;
        RequestServerSideAccess(serverClientId, forceRefreshToken);
        return await tcs.Task;
    }
}
