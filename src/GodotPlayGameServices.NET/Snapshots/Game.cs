#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Snapshots;

/// <summary>
/// Represents a Google Play Games game.
/// </summary>
public class Game
{
    /// <summary>Whether snapshots are enabled for this game.</summary>
    public bool AreSnapshotsEnabled { get; }

    /// <summary>The number of achievements registered for this game.</summary>
    public int AchievementTotalCount { get; }

    /// <summary>The application ID for this game.</summary>
    public string ApplicationId { get; }

    /// <summary>The description of this game.</summary>
    public string Description { get; }

    /// <summary>The name of the developer.</summary>
    public string DeveloperName { get; }

    /// <summary>The display name for this game.</summary>
    public string DisplayName { get; }

    /// <summary>The number of leaderboards registered for this game.</summary>
    public int LeaderboardCount { get; }

    /// <summary>The primary category of the game.</summary>
    public string PrimaryCategory { get; }

    /// <summary>The secondary category of the game.</summary>
    public string SecondaryCategory { get; }

    /// <summary>The theme color for this game.</summary>
    public string ThemeColor { get; }

    /// <summary>Whether this game supports gamepads.</summary>
    public bool HasGamepadSupport { get; }

    /// <summary>URI for the game's hi-res image.</summary>
    public string HiResImageUri { get; }

    /// <summary>URI for the game's icon image.</summary>
    public string IconImageUri { get; }

    /// <summary>URI for the game's featured (banner) image.</summary>
    public string FeaturedImageUri { get; }

    /// <summary>
    /// Creates a Game from a Godot Dictionary.
    /// </summary>
    public Game(Dictionary dictionary)
    {
        AreSnapshotsEnabled = dictionary.GetBoolOrDefault("areSnapshotsEnabled");
        AchievementTotalCount = dictionary.GetIntOrDefault("achievementTotalCount");
        ApplicationId = dictionary.GetStringOrDefault("applicationId");
        Description = dictionary.GetStringOrDefault("description");
        DeveloperName = dictionary.GetStringOrDefault("developerName");
        DisplayName = dictionary.GetStringOrDefault("displayName");
        LeaderboardCount = dictionary.GetIntOrDefault("leaderboardCount");
        PrimaryCategory = dictionary.GetStringOrDefault("primaryCategory");
        SecondaryCategory = dictionary.GetStringOrDefault("secondaryCategory");
        ThemeColor = dictionary.GetStringOrDefault("themeColor");
        HasGamepadSupport = dictionary.GetBoolOrDefault("hasGamepadSupport");
        HiResImageUri = dictionary.GetStringOrDefault("hiResImageUri");
        IconImageUri = dictionary.GetStringOrDefault("iconImageUri");
        FeaturedImageUri = dictionary.GetStringOrDefault("featuredImageUri");
    }

    public override string ToString()
        => $"Game(id={ApplicationId}, name={DisplayName})";
}
