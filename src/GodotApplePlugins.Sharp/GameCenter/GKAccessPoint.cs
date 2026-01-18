using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKAccessPoint GDExtension class.
/// Manages the Game Center access point UI element.
/// </summary>
public class GKAccessPoint
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKAccessPoint wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The GKAccessPoint GDExtension object.</param>
    public GKAccessPoint(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Controls visibility of the access point.
    /// </summary>
    public bool Active
    {
        get => _instance.Get(ApplePluginStringNames.Active).AsBool();
        set => _instance.Set(ApplePluginStringNames.Active, value);
    }

    /// <summary>
    /// Screen corner position for the access point.
    /// </summary>
    public GKAccessPointLocation Location
    {
        get => (GKAccessPointLocation)_instance.Get(ApplePluginStringNames.Location).AsInt32();
        set => _instance.Set(ApplePluginStringNames.Location, (int)value);
    }

    /// <summary>
    /// Toggles achievement/leaderboard highlights display.
    /// </summary>
    public bool ShowHighlights
    {
        get => _instance.Get(ApplePluginStringNames.ShowHighlights).AsBool();
        set => _instance.Set(ApplePluginStringNames.ShowHighlights, value);
    }

    /// <summary>
    /// Indicates if the access point is currently visible.
    /// </summary>
    public bool Visible => _instance.Get(ApplePluginStringNames.Visible).AsBool();

    /// <summary>
    /// Indicates if the Game Center dashboard is currently displayed.
    /// </summary>
    public bool IsPresentingGameCenter =>
        _instance.Get(ApplePluginStringNames.IsPresentingGameCenter).AsBool();

    /// <summary>
    /// Position and size of the access point in screen coordinates.
    /// </summary>
    public Rect2 FrameInScreenCoordinates =>
        _instance.Get(ApplePluginStringNames.FrameInScreenCoordinates).AsRect2();

    /// <summary>
    /// Displays the Game Center dashboard as if the player taps or presses the access point.
    /// </summary>
    /// <param name="done">Callback invoked when the dashboard is dismissed.</param>
    public void Trigger(Action? done = null)
    {
        var callable = done != null
            ? Callable.From(() => done())
            : Callable.From(() => { });
        _instance.Call(ApplePluginStringNames.Trigger, callable);
    }

    /// <summary>
    /// Displays the Game Center dashboard in the specified state.
    /// </summary>
    /// <param name="state">The state to display.</param>
    /// <param name="done">Callback invoked when the dashboard is dismissed.</param>
    public void TriggerWithState(int state, Action? done = null)
    {
        var callable = done != null
            ? Callable.From(() => done())
            : Callable.From(() => { });
        _instance.Call(ApplePluginStringNames.TriggerWithState, state, callable);
    }

    /// <summary>
    /// Shows the dashboard with a specific achievement.
    /// Requires macOS 15+, iOS 18+.
    /// </summary>
    /// <param name="achievementId">The achievement identifier.</param>
    /// <param name="done">Callback invoked when the dashboard is dismissed.</param>
    public void TriggerWithAchievement(string achievementId, Action? done = null)
    {
        var callable = done != null
            ? Callable.From(() => done())
            : Callable.From(() => { });
        _instance.Call(ApplePluginStringNames.TriggerWithAchievement, achievementId, callable);
    }

    /// <summary>
    /// Shows the dashboard with a specific leaderboard.
    /// Requires macOS 15+, iOS 18+.
    /// </summary>
    /// <param name="leaderboardId">The leaderboard identifier.</param>
    /// <param name="playerScope">The player scope (global or friends).</param>
    /// <param name="timeScope">The time scope (today, week, all time).</param>
    /// <param name="done">Callback invoked when the dashboard is dismissed.</param>
    public void TriggerWithLeaderboard(
        string leaderboardId,
        GKLeaderboardPlayerScope playerScope,
        GKLeaderboardTimeScope timeScope,
        Action? done = null)
    {
        var callable = done != null
            ? Callable.From(() => done())
            : Callable.From(() => { });
        _instance.Call(ApplePluginStringNames.TriggerWithLeaderboard,
            leaderboardId, (int)playerScope, (int)timeScope, callable);
    }

    /// <summary>
    /// Shows the dashboard with a leaderboard set.
    /// Requires macOS 15+, iOS 18+.
    /// </summary>
    /// <param name="leaderboardSetId">The leaderboard set identifier.</param>
    /// <param name="done">Callback invoked when the dashboard is dismissed.</param>
    public void TriggerWithLeaderboardSet(string leaderboardSetId, Action? done = null)
    {
        var callable = done != null
            ? Callable.From(() => done())
            : Callable.From(() => { });
        _instance.Call(ApplePluginStringNames.TriggerWithLeaderboardSet, leaderboardSetId, callable);
    }

    /// <summary>
    /// Shows the player profile dashboard.
    /// Requires macOS 15+, iOS 18+.
    /// </summary>
    /// <param name="player">The player to show.</param>
    /// <param name="done">Callback invoked when the dashboard is dismissed.</param>
    public void TriggerWithPlayer(GKPlayer player, Action? done = null)
    {
        var callable = done != null
            ? Callable.From(() => done())
            : Callable.From(() => { });
        _instance.Call(ApplePluginStringNames.TriggerWithPlayer, player.Instance, callable);
    }
}
