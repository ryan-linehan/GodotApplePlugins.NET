#nullable enable

using Godot;
using System.Threading.Tasks;

namespace GodotPlayGameServices.NET.Achievements;

/// <summary>
/// Client for Google Play Games achievements functionality.
/// </summary>
public class AchievementsClient
{
    private readonly GodotObject _plugin;

    /// <summary>
    /// Emitted after unlocking or incrementing an achievement.
    /// </summary>
    public event Action<bool, string>? AchievementUnlocked;

    /// <summary>
    /// Emitted after loading achievements from the server.
    /// </summary>
    public event Action<Achievement[]>? AchievementsLoaded;

    /// <summary>
    /// Emitted after revealing a hidden achievement.
    /// </summary>
    public event Action<bool, string>? AchievementRevealed;

    internal AchievementsClient(GodotObject plugin)
    {
        _plugin = plugin;

        _plugin.Connect("achievementUnlocked",
            Callable.From<bool, string>((isUnlocked, achievementId) =>
                AchievementUnlocked?.Invoke(isUnlocked, achievementId)));

        _plugin.Connect("achievementsLoaded",
            Callable.From<string>(json =>
            {
                var dicts = JsonHelper.ParseArray(json);
                var achievements = new Achievement[dicts.Length];
                for (int i = 0; i < dicts.Length; i++)
                    achievements[i] = new Achievement(dicts[i]);
                AchievementsLoaded?.Invoke(achievements);
            }));

        _plugin.Connect("achievementRevealed",
            Callable.From<bool, string>((isRevealed, achievementId) =>
                AchievementRevealed?.Invoke(isRevealed, achievementId)));
    }

    /// <summary>
    /// Increments an incremental achievement by the given number of steps.
    /// </summary>
    /// <param name="achievementId">The achievement ID.</param>
    /// <param name="amount">The number of steps to increment.</param>
    public void IncrementAchievement(string achievementId, int amount)
    {
        _plugin.Call("incrementAchievement", achievementId, amount);
    }

    /// <summary>
    /// Loads all achievements for the currently signed-in player.
    /// </summary>
    /// <param name="forceReload">If true, bypasses the local cache and fetches from the server.</param>
    public void LoadAchievements(bool forceReload)
    {
        _plugin.Call("loadAchievements", forceReload);
    }

    /// <summary>
    /// Reveals a hidden achievement to the player.
    /// </summary>
    /// <param name="achievementId">The achievement ID to reveal.</param>
    public void RevealAchievement(string achievementId)
    {
        _plugin.Call("revealAchievement", achievementId);
    }

    /// <summary>
    /// Opens the achievements UI.
    /// </summary>
    public void ShowAchievements()
    {
        _plugin.Call("showAchievements");
    }

    /// <summary>
    /// Immediately unlocks an achievement.
    /// </summary>
    /// <param name="achievementId">The achievement ID to unlock.</param>
    public void UnlockAchievement(string achievementId)
    {
        _plugin.Call("unlockAchievement", achievementId);
    }

    /// <summary>
    /// Sets the minimum number of completed steps for an incremental achievement.
    /// </summary>
    /// <param name="achievementId">The achievement ID.</param>
    /// <param name="amount">The minimum number of steps.</param>
    public void SetAchievementSteps(string achievementId, int amount)
    {
        _plugin.Call("setAchievementSteps", achievementId, amount);
    }

    /// <summary>
    /// Loads achievements and returns the result asynchronously.
    /// </summary>
    public async Task<Achievement[]> LoadAchievementsAsync(bool forceReload)
    {
        var tcs = new TaskCompletionSource<Achievement[]>();
        void Handler(Achievement[] achievements) { tcs.TrySetResult(achievements); AchievementsLoaded -= Handler; }
        AchievementsLoaded += Handler;
        LoadAchievements(forceReload);
        return await tcs.Task;
    }
}
