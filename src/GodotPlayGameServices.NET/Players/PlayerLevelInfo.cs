#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Players;

/// <summary>
/// Represents a player's level progression information.
/// </summary>
public class PlayerLevelInfo
{
    /// <summary>The player's current level.</summary>
    public PlayerLevel CurrentLevel { get; }

    /// <summary>The player's next level.</summary>
    public PlayerLevel NextLevel { get; }

    /// <summary>The player's current total XP.</summary>
    public long CurrentXpTotal { get; }

    /// <summary>The timestamp of the player's last level-up.</summary>
    public long LastLevelUpTimestamp { get; }

    /// <summary>Whether the player has reached the maximum level.</summary>
    public bool IsMaxLevel { get; }

    /// <summary>
    /// Creates a PlayerLevelInfo from a Godot Dictionary.
    /// </summary>
    public PlayerLevelInfo(Dictionary dictionary)
    {
        var currentLevelDict = dictionary.GetDictionaryOrNull("currentLevel");
        CurrentLevel = new PlayerLevel(currentLevelDict ?? new Dictionary());

        var nextLevelDict = dictionary.GetDictionaryOrNull("nextLevel");
        NextLevel = new PlayerLevel(nextLevelDict ?? new Dictionary());

        CurrentXpTotal = dictionary.GetLongOrDefault("currentXpTotal");
        LastLevelUpTimestamp = dictionary.GetLongOrDefault("lastLevelUpTimestamp");
        IsMaxLevel = dictionary.GetBoolOrDefault("isMaxLevel");
    }

    public override string ToString()
        => $"PlayerLevelInfo(xp={CurrentXpTotal}, level={CurrentLevel.LevelNumber}, maxLevel={IsMaxLevel})";
}
