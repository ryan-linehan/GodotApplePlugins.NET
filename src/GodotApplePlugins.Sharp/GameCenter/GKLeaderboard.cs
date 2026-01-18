using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKLeaderboard GDExtension class.
/// Represents a Game Center leaderboard.
/// </summary>
public class GKLeaderboard
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKLeaderboard wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The GKLeaderboard GDExtension object.</param>
    public GKLeaderboard(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Display name shown to players.
    /// </summary>
    public string Title => _instance.Get(ApplePluginStringNames.Title).AsString();

    /// <summary>
    /// Group ID configured in App Store Connect.
    /// </summary>
    public string GroupIdentifier => _instance.Get(ApplePluginStringNames.GroupIdentifier).AsString();

    /// <summary>
    /// Leaderboard type (classic, recurring, unknown).
    /// </summary>
    public GKLeaderboardType Type => (GKLeaderboardType)_instance.Get(ApplePluginStringNames.Type).AsInt32();

    /// <summary>
    /// Leaderboard start date (Unix timestamp).
    /// </summary>
    public double StartDate => _instance.Get(ApplePluginStringNames.StartDate).AsDouble();

    /// <summary>
    /// Next occurrence date for recurring leaderboards (Unix timestamp).
    /// </summary>
    public double NextStartDate => _instance.Get(ApplePluginStringNames.NextStartDate).AsDouble();

    /// <summary>
    /// Duration in seconds for recurring leaderboards.
    /// </summary>
    public double Duration => _instance.Get(ApplePluginStringNames.Duration).AsDouble();

    /// <summary>
    /// Fetches leaderboard metadata. Pass an empty array to get all leaderboards.
    /// </summary>
    /// <param name="leaderboardClass">The GKLeaderboard class object from the GDExtension.</param>
    /// <param name="ids">Leaderboard IDs to fetch, or empty for all.</param>
    /// <param name="callback">
    /// Callback invoked with the list of leaderboards and an optional error message.
    /// </param>
    public static void LoadLeaderboards(GodotObject leaderboardClass, string[] ids, Action<GKLeaderboard[], string?> callback)
    {
        var godotArray = new Godot.Collections.Array();
        foreach (var id in ids)
        {
            godotArray.Add(id);
        }

        var callable = Callable.From((Godot.Collections.Array leaderboards, Variant error) =>
        {
            var items = leaderboards
                .Select(lb => new GKLeaderboard((GodotObject)lb.Obj!))
                .ToArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(items, errorMsg);
        });
        leaderboardClass.Call(ApplePluginStringNames.LoadLeaderboards, godotArray, callable);
    }

    /// <summary>
    /// Submits a score to this leaderboard.
    /// </summary>
    /// <param name="score">The score to submit.</param>
    /// <param name="context">Additional context value.</param>
    /// <param name="player">The player submitting the score.</param>
    /// <param name="callback">
    /// Callback invoked with an optional error message. Null on success.
    /// </param>
    public void SubmitScore(long score, long context, GKPlayer player, Action<string?> callback)
    {
        var callable = Callable.From((Variant error) =>
        {
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(errorMsg);
        });
        _instance.Call(ApplePluginStringNames.SubmitScore, score, context, player.Instance, callable);
    }

    /// <summary>
    /// Loads leaderboard entries for the local player.
    /// </summary>
    /// <param name="playerScope">The player scope (global or friends).</param>
    /// <param name="timeScope">The time scope.</param>
    /// <param name="start">Starting rank (1-based).</param>
    /// <param name="length">Number of entries to fetch.</param>
    /// <param name="callback">
    /// Callback invoked with entries, local player entry, total count, and optional error.
    /// </param>
    public void LoadLocalPlayerEntries(
        GKLeaderboardPlayerScope playerScope,
        GKLeaderboardTimeScope timeScope,
        int start,
        int length,
        Action<GKLeaderboardEntry[], GKLeaderboardEntry?, int, string?> callback)
    {
        var callable = Callable.From((
            Godot.Collections.Array entries,
            Variant localEntry,
            int totalCount,
            Variant error) =>
        {
            var items = entries
                .Select(e => new GKLeaderboardEntry((GodotObject)e.Obj!))
                .ToArray();
            GKLeaderboardEntry? localPlayerEntry = null;
            if (localEntry.Obj is GodotObject obj)
            {
                localPlayerEntry = new GKLeaderboardEntry(obj);
            }
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(items, localPlayerEntry, totalCount, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.LoadLocalPlayerEntries,
            (int)playerScope, (int)timeScope, start, length, callable);
    }

    /// <summary>
    /// Downloads the leaderboard icon asynchronously.
    /// </summary>
    /// <param name="callback">
    /// Callback invoked with the image and an optional error message.
    /// </param>
    public void LoadImage(Action<Image?, string?> callback)
    {
        var callable = Callable.From((Variant image, Variant error) =>
        {
            Image? img = null;
            if (image.Obj is Image i)
            {
                img = i;
            }
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(img, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.LoadImage, callable);
    }
}

/// <summary>
/// C# wrapper for leaderboard entry data.
/// </summary>
public class GKLeaderboardEntry
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKLeaderboardEntry wrapper.
    /// </summary>
    /// <param name="instance">The GKLeaderboardEntry GDExtension object.</param>
    public GKLeaderboardEntry(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The player who owns this entry.
    /// </summary>
    public GKPlayer Player
    {
        get
        {
            var result = _instance.Get(ApplePluginStringNames.Player);
            return new GKPlayer((GodotObject)result.Obj!);
        }
    }

    /// <summary>
    /// The player's rank on the leaderboard.
    /// </summary>
    public int Rank => _instance.Get(new StringName("rank")).AsInt32();

    /// <summary>
    /// The player's score.
    /// </summary>
    public long Score => _instance.Get(new StringName("score")).AsInt64();

    /// <summary>
    /// The formatted score string.
    /// </summary>
    public string FormattedScore => _instance.Get(new StringName("formatted_score")).AsString();

    /// <summary>
    /// Additional context value.
    /// </summary>
    public long Context => _instance.Get(new StringName("context")).AsInt64();

    /// <summary>
    /// Date when the score was submitted (Unix timestamp).
    /// </summary>
    public double Date => _instance.Get(new StringName("date")).AsDouble();
}
