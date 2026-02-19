#nullable enable

using Godot.Collections;

namespace GodotPlayGameServices.NET.Snapshots;

/// <summary>
/// Represents a saved game snapshot containing both content and metadata.
/// </summary>
public class Snapshot
{
    /// <summary>
    /// Value indicating no display limit for the show_saved_games method.
    /// </summary>
    public const int DisplayLimitNone = -1;

    /// <summary>The binary content of the snapshot.</summary>
    public byte[] Content { get; }

    /// <summary>The metadata for this snapshot.</summary>
    public SnapshotMetadata? Metadata { get; }

    /// <summary>
    /// Creates a Snapshot from a Godot Dictionary.
    /// </summary>
    public Snapshot(Dictionary dictionary)
    {
        if (dictionary.ContainsKey("content"))
            Content = dictionary["content"].AsByteArray();
        else
            Content = [];

        var metadataDict = dictionary.GetDictionaryOrNull("metadata");
        Metadata = metadataDict != null ? new SnapshotMetadata(metadataDict) : null;
    }

    public override string ToString()
        => $"Snapshot(name={Metadata?.UniqueName}, contentSize={Content.Length})";
}
