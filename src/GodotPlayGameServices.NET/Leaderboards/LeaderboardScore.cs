#nullable enable

using Godot.Collections;
using GodotPlayGameServices.NET.Players;

namespace GodotPlayGameServices.NET.Leaderboards;

/// <summary>
/// Represents a player's score entry on a leaderboard.
/// </summary>
public class LeaderboardScore
{
    /// <summary>The formatted display rank.</summary>
    public string DisplayRank { get; }

    /// <summary>The formatted display score.</summary>
    public string DisplayScore { get; }

    /// <summary>The player's rank.</summary>
    public long Rank { get; }

    /// <summary>The raw numeric score.</summary>
    public long RawScore { get; }

    /// <summary>The player who holds this score.</summary>
    public Player? ScoreHolder { get; }

    /// <summary>The display name of the score holder.</summary>
    public string ScoreHolderDisplayName { get; }

    /// <summary>URI for the score holder's hi-res image.</summary>
    public string ScoreHolderHiResImageUri { get; }

    /// <summary>URI for the score holder's icon image.</summary>
    public string ScoreHolderIconImageUri { get; }

    /// <summary>An optional tag associated with the score.</summary>
    public string ScoreTag { get; }

    /// <summary>Timestamp when the score was achieved, in milliseconds since epoch.</summary>
    public long TimestampMillis { get; }

    /// <summary>
    /// Creates a LeaderboardScore from a Godot Dictionary.
    /// </summary>
    public LeaderboardScore(Dictionary dictionary)
    {
        DisplayRank = dictionary.GetStringOrDefault("displayRank");
        DisplayScore = dictionary.GetStringOrDefault("displayScore");
        Rank = dictionary.GetLongOrDefault("rank");
        RawScore = dictionary.GetLongOrDefault("rawScore");
        ScoreHolderDisplayName = dictionary.GetStringOrDefault("scoreHolderDisplayName");
        ScoreHolderHiResImageUri = dictionary.GetStringOrDefault("scoreHolderHiResImageUri");
        ScoreHolderIconImageUri = dictionary.GetStringOrDefault("scoreHolderIconImageUri");
        ScoreTag = dictionary.GetStringOrDefault("scoreTag");
        TimestampMillis = dictionary.GetLongOrDefault("timestampMillis");

        var holderDict = dictionary.GetDictionaryOrNull("scoreHolder");
        ScoreHolder = holderDict != null ? new Player(holderDict) : null;
    }

    public override string ToString()
        => $"LeaderboardScore(rank={Rank}, score={RawScore}, holder={ScoreHolderDisplayName})";
}
