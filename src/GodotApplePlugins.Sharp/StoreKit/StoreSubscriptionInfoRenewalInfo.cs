using Godot;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// C# wrapper for the StoreSubscriptionInfoRenewalInfo GDExtension class.
/// Contains verified renewal data for a subscription.
/// </summary>
public class StoreSubscriptionInfoRenewalInfo
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new StoreSubscriptionInfoRenewalInfo wrapper.
    /// </summary>
    public StoreSubscriptionInfoRenewalInfo(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The app account token used when purchasing, as a UUID string.
    /// Returns empty string when not set.
    /// </summary>
    public string AppAccountToken => _instance.Get(new StringName("app_account_token")).AsString();

    /// <summary>
    /// The app transaction identifier associated with the subscription.
    /// Returns empty string when unavailable.
    /// </summary>
    public string AppTransactionId => _instance.Get(new StringName("app_transaction_id")).AsString();

    /// <summary>
    /// The active product identifier within a subscription group.
    /// </summary>
    public string CurrentProductId => _instance.Get(new StringName("current_product_id")).AsString();

    /// <summary>
    /// The original transaction identifier for the subscription.
    /// Returns 0 when unavailable.
    /// </summary>
    public long OriginalTransactionId => _instance.Get(new StringName("original_transaction_id")).AsInt64();
}
