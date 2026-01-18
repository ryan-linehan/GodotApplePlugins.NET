using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// Game Center view controller states.
/// </summary>
public enum GKGameCenterViewState
{
    /// <summary>Default/initial view.</summary>
    DefaultScreen = 0,
    /// <summary>Leaderboard sets or leaderboards.</summary>
    Leaderboards = 1,
    /// <summary>Achievement list.</summary>
    Achievements = 2,
    /// <summary>Current player profile.</summary>
    LocalPlayerProfile = 3,
    /// <summary>Main dashboard.</summary>
    Dashboard = 4,
    /// <summary>Friends roster.</summary>
    LocalPlayerFriendsList = 5
}

/// <summary>
/// C# wrapper for the GKGameCenterViewController GDExtension class.
/// Presents Game Center dashboards using static "show_*" methods.
/// </summary>
public static class GKGameCenterViewController
{
    private static readonly StringName ShowAchievementMethod = new("show_achievement");
    private static readonly StringName ShowLeaderboardMethod = new("show_leaderboard");
    private static readonly StringName ShowLeaderboardTimePeriodMethod = new("show_leaderboard_time_period");
    private static readonly StringName ShowLeaderboardSetMethod = new("show_leaderboardset");
    private static readonly StringName ShowPlayerMethod = new("show_player");
    private static readonly StringName ShowTypeMethod = new("show_type");

    /// <summary>
    /// Displays a specific achievement by identifier.
    /// </summary>
    /// <param name="classInstance">The GKGameCenterViewController class instance.</param>
    /// <param name="achievementId">The achievement identifier.</param>
    public static void ShowAchievement(GodotObject classInstance, string achievementId)
    {
        classInstance.Call(ShowAchievementMethod, achievementId);
    }

    /// <summary>
    /// Presents a leaderboard with filtered players.
    /// </summary>
    /// <param name="classInstance">The GKGameCenterViewController class instance.</param>
    /// <param name="leaderboard">The leaderboard to show.</param>
    /// <param name="playerScope">The player scope (global or friends).</param>
    public static void ShowLeaderboard(GodotObject classInstance, GKLeaderboard leaderboard, GKLeaderboardPlayerScope playerScope)
    {
        classInstance.Call(ShowLeaderboardMethod, leaderboard.Instance, (int)playerScope);
    }

    /// <summary>
    /// Shows a leaderboard with scope and time filtering.
    /// </summary>
    /// <param name="classInstance">The GKGameCenterViewController class instance.</param>
    /// <param name="leaderboardId">The leaderboard identifier.</param>
    /// <param name="playerScope">The player scope (global or friends).</param>
    /// <param name="timeScope">The time scope (today, week, all time).</param>
    public static void ShowLeaderboardTimePeriod(
        GodotObject classInstance,
        string leaderboardId,
        GKLeaderboardPlayerScope playerScope,
        GKLeaderboardTimeScope timeScope)
    {
        classInstance.Call(ShowLeaderboardTimePeriodMethod, leaderboardId, (int)playerScope, (int)timeScope);
    }

    /// <summary>
    /// Displays a particular leaderboard set.
    /// </summary>
    /// <param name="classInstance">The GKGameCenterViewController class instance.</param>
    /// <param name="leaderboardSetId">The leaderboard set identifier.</param>
    public static void ShowLeaderboardSet(GodotObject classInstance, string leaderboardSetId)
    {
        classInstance.Call(ShowLeaderboardSetMethod, leaderboardSetId);
    }

    /// <summary>
    /// Presents a player's GameCenter profile.
    /// </summary>
    /// <param name="classInstance">The GKGameCenterViewController class instance.</param>
    /// <param name="player">The player to show.</param>
    public static void ShowPlayer(GodotObject classInstance, GKPlayer player)
    {
        classInstance.Call(ShowPlayerMethod, player.Instance);
    }

    /// <summary>
    /// Opens a Game Center dashboard by type.
    /// </summary>
    /// <param name="classInstance">The GKGameCenterViewController class instance.</param>
    /// <param name="state">The dashboard state to show.</param>
    public static void ShowType(GodotObject classInstance, GKGameCenterViewState state)
    {
        classInstance.Call(ShowTypeMethod, (int)state);
    }
}
