using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// Subscription renewal state.
/// </summary>
public enum SubscriptionRenewalState
{
    /// <summary>Renewal status unavailable.</summary>
    Unknown = 0,
    /// <summary>Subscription no longer active.</summary>
    Expired = 1,
    /// <summary>Currently active subscription.</summary>
    Subscribed = 2,
    /// <summary>Failed renewal with retry attempts underway.</summary>
    InBillingRetryPeriod = 3,
    /// <summary>Failed renewal with continued access.</summary>
    InGracePeriod = 4,
    /// <summary>Revoked by Apple.</summary>
    Revoked = 5
}

/// <summary>
/// C# wrapper for the StoreSubscriptionInfoStatus GDExtension class.
/// Contains subscription status including renewal state and latest transaction.
/// </summary>
public class StoreSubscriptionInfoStatus
{
    private readonly GodotObject _instance;
    private StoreSubscriptionInfoRenewalInfo? _renewalInfo;
    private StoreTransaction? _transaction;

    /// <summary>
    /// Creates a new StoreSubscriptionInfoStatus wrapper.
    /// </summary>
    public StoreSubscriptionInfoStatus(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Verified renewal details, or null if unavailable.
    /// </summary>
    public StoreSubscriptionInfoRenewalInfo? RenewalInfo
    {
        get
        {
            if (_renewalInfo != null) return _renewalInfo;

            var result = _instance.Get(new StringName("renewal_info"));
            if (result.Obj is GodotObject obj)
            {
                _renewalInfo = new StoreSubscriptionInfoRenewalInfo(obj);
            }
            return _renewalInfo;
        }
    }

    /// <summary>
    /// Current renewal state.
    /// </summary>
    public SubscriptionRenewalState State =>
        (SubscriptionRenewalState)_instance.Get(new StringName("state")).AsInt32();

    /// <summary>
    /// Latest verified transaction, or null if unavailable.
    /// </summary>
    public StoreTransaction? Transaction
    {
        get
        {
            if (_transaction != null) return _transaction;

            var result = _instance.Get(new StringName("transaction"));
            if (result.Obj is GodotObject obj)
            {
                _transaction = new StoreTransaction(obj);
            }
            return _transaction;
        }
    }

    /// <summary>
    /// Returns whether the subscription is currently active.
    /// </summary>
    public bool IsActive => State == SubscriptionRenewalState.Subscribed ||
                            State == SubscriptionRenewalState.InGracePeriod;
}
