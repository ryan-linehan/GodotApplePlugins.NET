#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Snapshots;

/// <summary>
/// Represents a conflict between two versions of a snapshot.
/// </summary>
public class SnapshotConflict
{
    /// <summary>The origin of the conflict, either "SAVE" or "LOAD".</summary>
    public string Origin { get; }

    /// <summary>The conflict ID.</summary>
    public string ConflictId { get; }

    /// <summary>The modified version of the snapshot involved in the conflict.</summary>
    public Snapshot? ConflictingSnapshot { get; }

    /// <summary>The most up-to-date server version of the snapshot.</summary>
    public Snapshot? ServerSnapshot { get; }

    /// <summary>
    /// Creates a SnapshotConflict from a Godot Dictionary.
    /// </summary>
    public SnapshotConflict(Dictionary dictionary)
    {
        Origin = dictionary.GetStringOrDefault("origin");
        ConflictId = dictionary.GetStringOrDefault("conflictId");

        var conflictingDict = dictionary.GetDictionaryOrNull("conflictingSnapshot");
        ConflictingSnapshot = conflictingDict != null ? new Snapshot(conflictingDict) : null;

        var serverDict = dictionary.GetDictionaryOrNull("serverSnapshot");
        ServerSnapshot = serverDict != null ? new Snapshot(serverDict) : null;
    }

    public override string ToString()
        => $"SnapshotConflict(origin={Origin}, conflictId={ConflictId})";
}
