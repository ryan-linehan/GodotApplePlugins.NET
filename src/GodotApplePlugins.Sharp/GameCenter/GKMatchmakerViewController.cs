using Godot;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKMatchmakerViewController GDExtension class.
/// Presents Apple's Game Center matchmaking UI.
/// </summary>
public class GKMatchmakerViewController
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Event raised when the user cancels matchmaking.
    /// </summary>
    public event Action<string>? Cancelled;

    /// <summary>
    /// Event raised when GameKit identifies players for hosted-server configurations.
    /// </summary>
    public event Action<GKPlayer[]>? DidFindHostedPlayers;

    /// <summary>
    /// Event raised when a peer-to-peer match becomes available.
    /// </summary>
    public event Action<GKMatch>? DidFindMatch;

    /// <summary>
    /// Event raised on Apple-reported matchmaking errors.
    /// </summary>
    public event Action<string>? FailedWithError;

    /// <summary>
    /// Creates a new GKMatchmakerViewController wrapper.
    /// </summary>
    public GKMatchmakerViewController(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ConnectSignals();
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Constructs a wrapper around Apple's view controller for the given match request.
    /// </summary>
    /// <param name="classInstance">The GKMatchmakerViewController class instance.</param>
    /// <param name="request">The match request configuration.</param>
    /// <returns>A new GKMatchmakerViewController wrapper.</returns>
    public static GKMatchmakerViewController CreateController(GodotObject classInstance, GKMatchRequest request)
    {
        var result = classInstance.Call(new StringName("create_controller"), request.Instance);
        return new GKMatchmakerViewController((GodotObject)result.Obj!);
    }

    /// <summary>
    /// Displays the previously created view controller.
    /// </summary>
    public void Present()
    {
        _instance.Call(new StringName("present"));
    }

    /// <summary>
    /// Launches the matchmaking interface and executes the callback with results.
    /// </summary>
    /// <param name="request">The match request configuration.</param>
    /// <param name="callback">Callback with (GKMatch match, string? error) - exactly one will be non-null.</param>
    public void RequestMatch(GKMatchRequest request, Action<GKMatch?, string?> callback)
    {
        var callable = Callable.From((Variant match, Variant error) =>
        {
            GKMatch? gkMatch = null;
            if (match.Obj is GodotObject obj)
            {
                gkMatch = new GKMatch(obj);
            }
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(gkMatch, errorMsg);
        });
        _instance.Call(new StringName("request_match"), request.Instance, callable);
    }

    private void ConnectSignals()
    {
        _instance.Connect(new StringName("cancelled"),
            Callable.From<string>(detail => Cancelled?.Invoke(detail)));

        _instance.Connect(new StringName("did_find_hosted_players"),
            Callable.From<Godot.Collections.Array>(players =>
            {
                var wrapped = players.Select(p => new GKPlayer((GodotObject)p.Obj!)).ToArray();
                DidFindHostedPlayers?.Invoke(wrapped);
            }));

        _instance.Connect(new StringName("did_find_match"),
            Callable.From<GodotObject>(match => DidFindMatch?.Invoke(new GKMatch(match))));

        _instance.Connect(new StringName("failed_with_error"),
            Callable.From<string>(message => FailedWithError?.Invoke(message)));
    }
}
