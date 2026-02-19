#nullable enable

using Godot.Collections;
using GodotPlayGameServices.NET.Players;

namespace GodotPlayGameServices.NET.Achievements;

/// <summary>
/// Represents a Google Play Games achievement.
/// </summary>
public class Achievement
{
    /// <summary>The unique ID of the achievement.</summary>
    public string AchievementId { get; }

    /// <summary>The name of the achievement.</summary>
    public string AchievementName { get; }

    /// <summary>The description of the achievement.</summary>
    public string Description { get; }

    /// <summary>The type of the achievement (standard or incremental).</summary>
    public AchievementType Type { get; }

    /// <summary>The current state of the achievement for the player.</summary>
    public AchievementState State { get; }

    /// <summary>The XP value of this achievement.</summary>
    public long XpValue { get; }

    /// <summary>URI for the revealed achievement image.</summary>
    public string RevealedImageUri { get; }

    /// <summary>URI for the unlocked achievement image.</summary>
    public string UnlockedImageUri { get; }

    /// <summary>The current number of steps completed (for incremental achievements).</summary>
    public int CurrentSteps { get; }

    /// <summary>The total number of steps required (for incremental achievements).</summary>
    public int TotalSteps { get; }

    /// <summary>Locale-formatted string of current steps.</summary>
    public string FormattedCurrentSteps { get; }

    /// <summary>Locale-formatted string of total steps.</summary>
    public string FormattedTotalSteps { get; }

    /// <summary>The player associated with this achievement.</summary>
    public Player? Player { get; }

    /// <summary>The timestamp when this achievement was last updated.</summary>
    public long LastUpdatedTimestamp { get; }

    /// <summary>
    /// Creates an Achievement from a Godot Dictionary (parsed from JSON).
    /// </summary>
    public Achievement(Dictionary dictionary)
    {
        AchievementId = dictionary.GetStringOrDefault("achievementId");
        AchievementName = dictionary.GetStringOrDefault("achievementName");
        Description = dictionary.GetStringOrDefault("description");
        Type = (AchievementType)dictionary.GetIntOrDefault("type");
        State = (AchievementState)dictionary.GetIntOrDefault("state");
        XpValue = dictionary.GetLongOrDefault("xpValue");
        RevealedImageUri = dictionary.GetStringOrDefault("revealedImageUri");
        UnlockedImageUri = dictionary.GetStringOrDefault("unlockedImageUri");
        CurrentSteps = dictionary.GetIntOrDefault("currentSteps");
        TotalSteps = dictionary.GetIntOrDefault("totalSteps");
        FormattedCurrentSteps = dictionary.GetStringOrDefault("formattedCurrentSteps");
        FormattedTotalSteps = dictionary.GetStringOrDefault("formattedTotalSteps");
        LastUpdatedTimestamp = dictionary.GetLongOrDefault("lastUpdatedTimestamp");

        var playerDict = dictionary.GetDictionaryOrNull("player");
        Player = playerDict != null ? new Player(playerDict) : null;
    }

    public override string ToString()
        => $"Achievement(id={AchievementId}, name={AchievementName}, state={State})";
}
