using Godot;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// Match type for Game Center multiplayer.
/// </summary>
public enum GKMatchType
{
    /// <summary>Peer-to-peer match.</summary>
    PeerToPeer = 0,
    /// <summary>Hosted match.</summary>
    Hosted = 1,
    /// <summary>Turn-based match.</summary>
    TurnBased = 2
}

/// <summary>
/// C# wrapper for the GKMatchRequest GDExtension class.
/// Configures player recruitment parameters for Game Center realtime matches.
/// </summary>
public class GKMatchRequest
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKMatchRequest wrapper.
    /// </summary>
    public GKMatchRequest(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Initial player count suggestion for the matchmaking UI.
    /// </summary>
    public int DefaultNumberOfPlayers
    {
        get => _instance.Get(new StringName("default_number_of_players")).AsInt32();
        set => _instance.Set(new StringName("default_number_of_players"), value);
    }

    /// <summary>
    /// Optional text displayed when players send invitations.
    /// </summary>
    public string InviteMessage
    {
        get => _instance.Get(new StringName("invite_message")).AsString();
        set => _instance.Set(new StringName("invite_message"), value);
    }

    /// <summary>
    /// Upper limit of desired participants.
    /// </summary>
    public int MaxPlayers
    {
        get => _instance.Get(new StringName("max_players")).AsInt32();
        set => _instance.Set(new StringName("max_players"), value);
    }

    /// <summary>
    /// Minimum players needed to start the match.
    /// </summary>
    public int MinPlayers
    {
        get => _instance.Get(new StringName("min_players")).AsInt32();
        set => _instance.Set(new StringName("min_players"), value);
    }

    /// <summary>
    /// Returns Apple's maximum supported player count for a specified match type.
    /// </summary>
    /// <param name="classInstance">The GKMatchRequest class instance.</param>
    /// <param name="matchType">The match type to query.</param>
    /// <returns>Maximum allowed players.</returns>
    public static int MaxPlayersAllowedForMatch(GodotObject classInstance, GKMatchType matchType)
    {
        return classInstance.Call(new StringName("max_players_allowed_for_match"), (int)matchType).AsInt32();
    }
}
