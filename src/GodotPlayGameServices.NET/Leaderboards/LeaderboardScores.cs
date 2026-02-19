#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Leaderboards;

/// <summary>
/// Represents a collection of leaderboard scores along with the leaderboard they belong to.
/// </summary>
public class LeaderboardScores
{
    /// <summary>The leaderboard these scores belong to.</summary>
    public Leaderboard Leaderboard { get; }

    /// <summary>The score entries.</summary>
    public LeaderboardScore[] Scores { get; }

    /// <summary>
    /// Creates a LeaderboardScores from a Godot Dictionary.
    /// </summary>
    public LeaderboardScores(Dictionary dictionary)
    {
        var leaderboardDict = dictionary.GetDictionaryOrNull("leaderboard");
        Leaderboard = new Leaderboard(leaderboardDict ?? new Dictionary());

        var scoresList = new List<LeaderboardScore>();
        if (dictionary.ContainsKey("scores"))
        {
            var scoresArray = dictionary["scores"].AsGodotArray();
            foreach (var score in scoresArray)
            {
                scoresList.Add(new LeaderboardScore(score.AsGodotDictionary()));
            }
        }
        Scores = scoresList.ToArray();
    }

    public override string ToString()
        => $"LeaderboardScores(leaderboard={Leaderboard.DisplayName}, count={Scores.Length})";
}
