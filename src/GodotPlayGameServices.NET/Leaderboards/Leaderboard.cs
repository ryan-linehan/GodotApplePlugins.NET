#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Leaderboards;

/// <summary>
/// Represents a Google Play Games leaderboard.
/// </summary>
public class Leaderboard
{
    /// <summary>The unique ID of the leaderboard.</summary>
    public string LeaderboardId { get; }

    /// <summary>The display name of the leaderboard.</summary>
    public string DisplayName { get; }

    /// <summary>URI for the leaderboard icon image.</summary>
    public string IconImageUri { get; }

    /// <summary>The score ordering for this leaderboard.</summary>
    public LeaderboardScoreOrder ScoreOrder { get; }

    /// <summary>The variants of this leaderboard (different time spans and collections).</summary>
    public LeaderboardVariant[] Variants { get; }

    /// <summary>
    /// Creates a Leaderboard from a Godot Dictionary (parsed from JSON).
    /// </summary>
    public Leaderboard(Dictionary dictionary)
    {
        LeaderboardId = dictionary.GetStringOrDefault("leaderboardId");
        DisplayName = dictionary.GetStringOrDefault("displayName");
        IconImageUri = dictionary.GetStringOrDefault("iconImageUri");
        ScoreOrder = (LeaderboardScoreOrder)dictionary.GetIntOrDefault("scoreOrder");

        var variantsList = new List<LeaderboardVariant>();
        if (dictionary.ContainsKey("variants"))
        {
            var variantsArray = dictionary["variants"].AsGodotArray();
            foreach (var variant in variantsArray)
            {
                variantsList.Add(new LeaderboardVariant(variant.AsGodotDictionary()));
            }
        }
        Variants = variantsList.ToArray();
    }

    public override string ToString()
        => $"Leaderboard(id={LeaderboardId}, name={DisplayName})";
}
