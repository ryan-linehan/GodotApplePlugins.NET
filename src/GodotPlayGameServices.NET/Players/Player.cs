#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Players;

/// <summary>
/// Represents a Google Play Games player.
/// </summary>
public class Player
{
    /// <summary>The player's unique ID.</summary>
    public string PlayerId { get; }

    /// <summary>The player's display name.</summary>
    public string DisplayName { get; }

    /// <summary>The player's title.</summary>
    public string Title { get; }

    /// <summary>URI for the player's hi-res profile image.</summary>
    public string HiResImageUri { get; }

    /// <summary>URI for the player's icon image.</summary>
    public string IconImageUri { get; }

    /// <summary>URI for the player's landscape banner image.</summary>
    public string BannerImageLandscapeUri { get; }

    /// <summary>URI for the player's portrait banner image.</summary>
    public string BannerImagePortraitUri { get; }

    /// <summary>Whether the player has a hi-res image.</summary>
    public bool HasHiResImage { get; }

    /// <summary>Whether the player has an icon image.</summary>
    public bool HasIconImage { get; }

    /// <summary>Timestamp when this player data was retrieved.</summary>
    public long RetrievedTimestamp { get; }

    /// <summary>The visibility status of this player's friends list.</summary>
    public FriendsListVisibilityStatus FriendsListVisibilityStatus { get; }

    /// <summary>The friend status of this player relative to the current player.</summary>
    public PlayerFriendStatus FriendStatus { get; }

    /// <summary>The player's level information, if available.</summary>
    public PlayerLevelInfo? LevelInfo { get; }

    /// <summary>
    /// Creates a Player from a Godot Dictionary (parsed from JSON).
    /// </summary>
    public Player(Dictionary dictionary)
    {
        PlayerId = dictionary.GetStringOrDefault("playerId");
        DisplayName = dictionary.GetStringOrDefault("displayName");
        Title = dictionary.GetStringOrDefault("title");
        HiResImageUri = dictionary.GetStringOrDefault("hiResImageUri");
        IconImageUri = dictionary.GetStringOrDefault("iconImageUri");
        BannerImageLandscapeUri = dictionary.GetStringOrDefault("bannerImageLandscapeUri");
        BannerImagePortraitUri = dictionary.GetStringOrDefault("bannerImagePortraitUri");
        HasHiResImage = dictionary.GetBoolOrDefault("hasHiResImage");
        HasIconImage = dictionary.GetBoolOrDefault("hasIconImage");
        RetrievedTimestamp = dictionary.GetLongOrDefault("retrievedTimestamp");
        FriendsListVisibilityStatus = (FriendsListVisibilityStatus)dictionary.GetIntOrDefault("friendsListVisibilityStatus");
        FriendStatus = (PlayerFriendStatus)dictionary.GetIntOrDefault("friendStatus", -1);

        var levelInfoDict = dictionary.GetDictionaryOrNull("levelInfo");
        LevelInfo = levelInfoDict != null ? new PlayerLevelInfo(levelInfoDict) : null;
    }

    public override string ToString()
        => $"Player(id={PlayerId}, name={DisplayName})";
}
