#nullable enable

namespace GodotPlayGameServices.NET;

/// <summary>
/// The type of an achievement.
/// </summary>
public enum AchievementType
{
    /// <summary>A standard achievement that is unlocked in a single step.</summary>
    Standard = 0,
    /// <summary>An incremental achievement that is unlocked after reaching a target number of steps.</summary>
    Incremental = 1
}

/// <summary>
/// The state of an achievement for the current player.
/// </summary>
public enum AchievementState
{
    /// <summary>The achievement has been unlocked by the player.</summary>
    Unlocked = 0,
    /// <summary>The achievement is revealed but not yet unlocked.</summary>
    Revealed = 1,
    /// <summary>The achievement is hidden from the player.</summary>
    Hidden = 2
}

/// <summary>
/// The score ordering for a leaderboard.
/// </summary>
public enum LeaderboardScoreOrder
{
    /// <summary>Scores are sorted in ascending order (lower is better).</summary>
    SmallerIsBetter = 0,
    /// <summary>Scores are sorted in descending order (higher is better).</summary>
    LargerIsBetter = 1
}

/// <summary>
/// The time span for leaderboard scores.
/// </summary>
public enum LeaderboardTimeSpan
{
    /// <summary>Scores reset daily.</summary>
    Daily = 0,
    /// <summary>Scores reset weekly.</summary>
    Weekly = 1,
    /// <summary>All-time scores.</summary>
    AllTime = 2
}

/// <summary>
/// The collection type for leaderboard scores.
/// </summary>
public enum LeaderboardCollection
{
    /// <summary>Public leaderboard visible to all players.</summary>
    Public = 0,
    /// <summary>Leaderboard visible only to friends.</summary>
    Friends = 3
}

/// <summary>
/// The visibility status of a player's friends list.
/// </summary>
public enum FriendsListVisibilityStatus
{
    /// <summary>The friends list visibility is unknown.</summary>
    Unknown = 0,
    /// <summary>The friends list is visible to the game.</summary>
    Visible = 1,
    /// <summary>The game must request access to the friends list.</summary>
    RequestRequired = 2,
    /// <summary>The friends list feature is unavailable for this game.</summary>
    FeatureUnavailable = 3
}

/// <summary>
/// The friend status between two players.
/// </summary>
public enum PlayerFriendStatus
{
    /// <summary>The friend status is unknown.</summary>
    Unknown = -1,
    /// <summary>The players have no relationship.</summary>
    NoRelationship = 0,
    /// <summary>The players are friends.</summary>
    Friend = 4
}
