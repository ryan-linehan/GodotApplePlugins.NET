#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Players;

/// <summary>
/// Represents a player level in Google Play Games.
/// </summary>
public class PlayerLevel
{
    /// <summary>The number for this level.</summary>
    public int LevelNumber { get; }

    /// <summary>The maximum XP value represented by this level, exclusive.</summary>
    public long MaxXp { get; }

    /// <summary>The minimum XP value needed to attain this level, inclusive.</summary>
    public long MinXp { get; }

    /// <summary>
    /// Creates a PlayerLevel from a Godot Dictionary.
    /// </summary>
    public PlayerLevel(Dictionary dictionary)
    {
        LevelNumber = dictionary.GetIntOrDefault("levelNumber");
        MaxXp = dictionary.GetLongOrDefault("maxXp");
        MinXp = dictionary.GetLongOrDefault("minXp");
    }

    public override string ToString()
        => $"PlayerLevel(level={LevelNumber}, xp={MinXp}-{MaxXp})";
}
