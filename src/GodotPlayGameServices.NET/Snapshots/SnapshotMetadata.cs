#nullable enable

using Godot.Collections;
using GodotPlayGameServices.NET.Players;

namespace GodotPlayGameServices.NET.Snapshots;

/// <summary>
/// Represents metadata for a saved game snapshot.
/// </summary>
public class SnapshotMetadata
{
    /// <summary>The unique ID of the snapshot.</summary>
    public string SnapshotId { get; }

    /// <summary>The unique name (file name) of the snapshot.</summary>
    public string UniqueName { get; }

    /// <summary>The description of the snapshot.</summary>
    public string Description { get; }

    /// <summary>The aspect ratio of the cover image.</summary>
    public int CoverImageAspectRatio { get; }

    /// <summary>The progress value of the snapshot.</summary>
    public long ProgressValue { get; }

    /// <summary>The last modification timestamp in milliseconds since epoch.</summary>
    public long LastModifiedTimestamp { get; }

    /// <summary>The played time in milliseconds.</summary>
    public long PlayedTime { get; }

    /// <summary>Whether there are pending changes not yet uploaded to the server.</summary>
    public bool HasChangePending { get; }

    /// <summary>The player who owns this snapshot.</summary>
    public Player? Owner { get; }

    /// <summary>The game this snapshot belongs to.</summary>
    public Game? GameInfo { get; }

    /// <summary>The name of the device that wrote this snapshot.</summary>
    public string DeviceName { get; }

    /// <summary>URI for the snapshot's cover image.</summary>
    public string CoverImageUri { get; }

    /// <summary>
    /// Creates a SnapshotMetadata from a Godot Dictionary.
    /// </summary>
    public SnapshotMetadata(Dictionary dictionary)
    {
        SnapshotId = dictionary.GetStringOrDefault("snapshotId");
        UniqueName = dictionary.GetStringOrDefault("uniqueName");
        Description = dictionary.GetStringOrDefault("description");
        CoverImageAspectRatio = dictionary.GetIntOrDefault("coverImageAspectRatio");
        ProgressValue = dictionary.GetLongOrDefault("progressValue");
        LastModifiedTimestamp = dictionary.GetLongOrDefault("lastModifiedTimestamp");
        PlayedTime = dictionary.GetLongOrDefault("playedTime");
        HasChangePending = dictionary.GetBoolOrDefault("hasChangePending");
        DeviceName = dictionary.GetStringOrDefault("deviceName");
        CoverImageUri = dictionary.GetStringOrDefault("coverImageUri");

        var ownerDict = dictionary.GetDictionaryOrNull("owner");
        Owner = ownerDict != null ? new Player(ownerDict) : null;

        var gameDict = dictionary.GetDictionaryOrNull("game");
        GameInfo = gameDict != null ? new Game(gameDict) : null;
    }

    public override string ToString()
        => $"SnapshotMetadata(id={SnapshotId}, name={UniqueName})";
}
