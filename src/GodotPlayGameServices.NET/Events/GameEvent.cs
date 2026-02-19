#nullable enable

using Godot.Collections;
using GodotPlayGameServices.NET.Players;

namespace GodotPlayGameServices.NET.Events;

/// <summary>
/// Represents a Google Play Games event.
/// </summary>
public class GameEvent
{
    /// <summary>The unique ID of the event.</summary>
    public string EventId { get; }

    /// <summary>The name of the event.</summary>
    public string Name { get; }

    /// <summary>The description of the event.</summary>
    public string Description { get; }

    /// <summary>A locale-formatted string of the event's cumulative value.</summary>
    public string FormattedValue { get; }

    /// <summary>URI for the event icon image.</summary>
    public string IconImageUri { get; }

    /// <summary>The cumulative value of this event (total increment count).</summary>
    public long Value { get; }

    /// <summary>Whether this event is visible.</summary>
    public bool IsVisible { get; }

    /// <summary>The player associated with this event.</summary>
    public Player? Player { get; }

    /// <summary>
    /// Creates a GameEvent from a Godot Dictionary (parsed from JSON).
    /// </summary>
    public GameEvent(Dictionary dictionary)
    {
        EventId = dictionary.GetStringOrDefault("eventId");
        Name = dictionary.GetStringOrDefault("name");
        Description = dictionary.GetStringOrDefault("description");
        FormattedValue = dictionary.GetStringOrDefault("formattedValue");
        IconImageUri = dictionary.GetStringOrDefault("iconImageUri");
        Value = dictionary.GetLongOrDefault("value");
        IsVisible = dictionary.GetBoolOrDefault("isVisible");

        var playerDict = dictionary.GetDictionaryOrNull("player");
        Player = playerDict != null ? new Player(playerDict) : null;
    }

    public override string ToString()
        => $"GameEvent(id={EventId}, name={Name}, value={Value})";
}
