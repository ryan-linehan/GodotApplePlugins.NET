using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// Send data mode for GKMatch networking.
/// </summary>
public enum GKMatchSendDataMode
{
    /// <summary>Uses GameKit's reliable data channel (ordered delivery with retries).</summary>
    Reliable = 0,
    /// <summary>Uses GameKit's unreliable channel, useful for latency-sensitive updates.</summary>
    Unreliable = 1
}

/// <summary>
/// C# wrapper for the GKMatch GDExtension class.
/// Represents an active real-time Game Center match.
/// </summary>
public class GKMatch
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Event raised when data is received from another player.
    /// </summary>
    public event Action<byte[], GKPlayer>? DataReceived;

    /// <summary>
    /// Event raised when data is received for a specific recipient.
    /// </summary>
    public event Action<byte[], GKPlayer, GKPlayer>? DataReceivedForRecipient;

    /// <summary>
    /// Event raised when a networking error occurs.
    /// </summary>
    public event Action<string>? DidFailWithError;

    /// <summary>
    /// Event raised when a player's connection status changes.
    /// </summary>
    public event Action<GKPlayer, bool>? PlayerChanged;

    /// <summary>
    /// Creates a new GKMatch wrapper.
    /// </summary>
    public GKMatch(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ConnectSignals();
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The number of additional players that GameKit is recruiting before the match can begin.
    /// </summary>
    public int ExpectedPlayerCount => _instance.Get(new StringName("expected_player_count")).AsInt32();

    /// <summary>
    /// Array of GKPlayer instances that are currently connected.
    /// </summary>
    public GKPlayer[] Players
    {
        get
        {
            var array = _instance.Get(new StringName("players")).AsGodotArray();
            return array.Select(p => new GKPlayer((GodotObject)p.Obj!)).ToArray();
        }
    }

    /// <summary>
    /// Leaves the match immediately.
    /// </summary>
    public void Disconnect()
    {
        _instance.Call(new StringName("disconnect"));
    }

    /// <summary>
    /// Sends data to specified players.
    /// </summary>
    /// <param name="data">The data to send.</param>
    /// <param name="toPlayers">The players to send to.</param>
    /// <param name="dataMode">The send mode (reliable or unreliable).</param>
    /// <returns>Error.Ok on success, Error.Failed otherwise.</returns>
    public Error Send(byte[] data, GKPlayer[] toPlayers, GKMatchSendDataMode dataMode)
    {
        var playersArray = new Godot.Collections.Array();
        foreach (var player in toPlayers)
        {
            playersArray.Add(player.Instance);
        }
        var result = _instance.Call(new StringName("send"), data, playersArray, (int)dataMode);
        return (Error)result.AsInt32();
    }

    /// <summary>
    /// Broadcasts data to all match participants.
    /// </summary>
    /// <param name="data">The data to send.</param>
    /// <param name="dataMode">The send mode (reliable or unreliable).</param>
    /// <returns>Error.Ok on success, Error.Failed otherwise.</returns>
    public Error SendDataToAllPlayers(byte[] data, GKMatchSendDataMode dataMode)
    {
        var result = _instance.Call(new StringName("send_data_to_all_players"), data, (int)dataMode);
        return (Error)result.AsInt32();
    }

    /// <summary>
    /// Sets a callback to determine if a disconnected player should be reinvited.
    /// </summary>
    /// <param name="callback">Callback receiving GKPlayer, return true to reinvite.</param>
    public void SetShouldReinviteDisconnectedPlayer(Func<GKPlayer, bool> callback)
    {
        var callable = Callable.From((GodotObject player) => callback(new GKPlayer(player)));
        _instance.Set(new StringName("should_reinvite_disconnected_player"), callable);
    }

    private void ConnectSignals()
    {
        _instance.Connect(new StringName("data_received"),
            Callable.From<byte[], GodotObject>((data, player) =>
                DataReceived?.Invoke(data, new GKPlayer(player))));

        _instance.Connect(new StringName("data_received_for_recipient_from_player"),
            Callable.From<byte[], GodotObject, GodotObject>((data, recipient, sender) =>
                DataReceivedForRecipient?.Invoke(data, new GKPlayer(recipient), new GKPlayer(sender))));

        _instance.Connect(new StringName("did_fail_with_error"),
            Callable.From<string>(error => DidFailWithError?.Invoke(error)));

        _instance.Connect(new StringName("player_changed"),
            Callable.From<GodotObject, bool>((player, connected) =>
                PlayerChanged?.Invoke(new GKPlayer(player), connected)));
    }
}
