#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Leaderboards;

/// <summary>
/// Represents a specific variant of a leaderboard (a combination of time span and collection).
/// </summary>
public class LeaderboardVariant
{
    /// <summary>The formatted display rank of the player.</summary>
    public string DisplayPlayerRank { get; }

    /// <summary>The formatted display score of the player.</summary>
    public string DisplayPlayerScore { get; }

    /// <summary>The number of scores in this leaderboard variant.</summary>
    public long NumScores { get; }

    /// <summary>The player's rank in this variant.</summary>
    public long PlayerRank { get; }

    /// <summary>The player's raw score in this variant.</summary>
    public long RawPlayerScore { get; }

    /// <summary>Whether player score information is available.</summary>
    public bool HasPlayerInfo { get; }

    /// <summary>The collection type for this variant.</summary>
    public LeaderboardCollection Collection { get; }

    /// <summary>The time span for this variant.</summary>
    public LeaderboardTimeSpan TimeSpan { get; }

    /// <summary>
    /// Creates a LeaderboardVariant from a Godot Dictionary.
    /// </summary>
    public LeaderboardVariant(Dictionary dictionary)
    {
        DisplayPlayerRank = dictionary.GetStringOrDefault("displayPlayerRank");
        DisplayPlayerScore = dictionary.GetStringOrDefault("displayPlayerScore");
        NumScores = dictionary.GetLongOrDefault("numScores");
        PlayerRank = dictionary.GetLongOrDefault("playerRank");
        RawPlayerScore = dictionary.GetLongOrDefault("rawPlayerScore");
        HasPlayerInfo = dictionary.GetBoolOrDefault("hasPlayerInfo");
        Collection = (LeaderboardCollection)dictionary.GetIntOrDefault("collection");
        TimeSpan = (LeaderboardTimeSpan)dictionary.GetIntOrDefault("timeSpan");
    }

    public override string ToString()
        => $"LeaderboardVariant(timeSpan={TimeSpan}, collection={Collection}, rank={PlayerRank})";
}
