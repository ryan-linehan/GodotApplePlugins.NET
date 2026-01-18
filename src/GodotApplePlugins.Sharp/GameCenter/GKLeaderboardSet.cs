using Godot;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKLeaderboardSet GDExtension class.
/// Groups related Apple Game Center leaderboards.
/// Note: The current binding only exposes the opaque object for reference purposes.
/// </summary>
public class GKLeaderboardSet
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKLeaderboardSet wrapper.
    /// </summary>
    public GKLeaderboardSet(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;
}
