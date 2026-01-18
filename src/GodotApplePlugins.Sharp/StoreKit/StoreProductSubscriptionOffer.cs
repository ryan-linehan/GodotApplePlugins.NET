using Godot;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// Subscription offer type.
/// </summary>
public enum SubscriptionOfferType
{
    /// <summary>An introductory offer for new subscribers.</summary>
    Introductory = 0,
    /// <summary>A promotional offer for existing or lapsed subscribers.</summary>
    Promotional = 1,
    /// <summary>A win-back offer for lapsed subscribers.</summary>
    WinBack = 2,
    /// <summary>An unknown offer type.</summary>
    Unknown = 3
}

/// <summary>
/// C# wrapper for the StoreProductSubscriptionOffer GDExtension class.
/// Represents a subscription offer associated with a StoreProduct.
/// </summary>
public class StoreProductSubscriptionOffer
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new StoreProductSubscriptionOffer wrapper.
    /// </summary>
    public StoreProductSubscriptionOffer(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;
}
