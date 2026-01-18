using Godot;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// Subscription period unit.
/// </summary>
public enum SubscriptionPeriodUnit
{
    /// <summary>A subscription period of one day.</summary>
    Day = 0,
    /// <summary>A subscription period of one month.</summary>
    Month = 1,
    /// <summary>A subscription period of one week.</summary>
    Week = 2,
    /// <summary>A subscription period of one year.</summary>
    Year = 3
}

/// <summary>
/// C# wrapper for the StoreProductSubscriptionPeriod GDExtension class.
/// Represents the duration of time between subscription renewals.
/// </summary>
public class StoreProductSubscriptionPeriod
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new StoreProductSubscriptionPeriod wrapper.
    /// </summary>
    public StoreProductSubscriptionPeriod(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;
}
