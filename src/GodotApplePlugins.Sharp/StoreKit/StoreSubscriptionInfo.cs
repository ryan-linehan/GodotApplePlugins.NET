using Godot;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// C# wrapper for the StoreSubscriptionInfo GDExtension class.
/// Provides subscription group metadata for a product.
/// </summary>
public class StoreSubscriptionInfo
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new StoreSubscriptionInfo wrapper.
    /// </summary>
    public StoreSubscriptionInfo(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The localized display name of the subscription group (empty if unavailable).
    /// </summary>
    public string GroupDisplayName => _instance.Get(new StringName("group_display_name")).AsString();

    /// <summary>
    /// Subscription group level for upgrade/downgrade ranking (-1 if unavailable).
    /// </summary>
    public int GroupLevel => _instance.Get(new StringName("group_level")).AsInt32();

    /// <summary>
    /// The subscription group identifier from App Store Connect (empty if unavailable).
    /// </summary>
    public string SubscriptionGroupId => _instance.Get(new StringName("subscription_group_id")).AsString();

    /// <summary>
    /// Fetches current status entries for the subscription.
    /// </summary>
    /// <param name="callback">Callback with array of status objects and optional error message.</param>
    public void GetStatus(Action<StoreSubscriptionInfoStatus[], string?> callback)
    {
        var callable = Callable.From((Variant result, Variant error) =>
        {
            if (error.VariantType != Variant.Type.Nil)
            {
                callback(Array.Empty<StoreSubscriptionInfoStatus>(), error.AsString());
                return;
            }

            var array = result.AsGodotArray();
            var statuses = array
                .Select(s => new StoreSubscriptionInfoStatus((GodotObject)s.Obj!))
                .ToArray();
            callback(statuses, null);
        });
        _instance.Call(new StringName("getStatus"), callable);
    }

    /// <summary>
    /// Retrieves status entries for a specific subscription group identifier.
    /// </summary>
    /// <param name="groupId">The subscription group ID.</param>
    /// <param name="callback">Callback with array of status objects and optional error message.</param>
    public void StatusForGroupId(string groupId, Action<StoreSubscriptionInfoStatus[], string?> callback)
    {
        var callable = Callable.From((Variant result, Variant error) =>
        {
            if (error.VariantType != Variant.Type.Nil)
            {
                callback(Array.Empty<StoreSubscriptionInfoStatus>(), error.AsString());
                return;
            }

            var array = result.AsGodotArray();
            var statuses = array
                .Select(s => new StoreSubscriptionInfoStatus((GodotObject)s.Obj!))
                .ToArray();
            callback(statuses, null);
        });
        _instance.Call(new StringName("status_for_group_id"), groupId, callable);
    }

    /// <summary>
    /// Fetches the status entry for a specific transaction ID.
    /// Note: macOS support requires version 15.4 or newer.
    /// </summary>
    /// <param name="transactionId">The transaction ID.</param>
    /// <param name="callback">Callback with status object (or null if not found) and optional error message.</param>
    public void StatusForTransaction(long transactionId, Action<StoreSubscriptionInfoStatus?, string?> callback)
    {
        var callable = Callable.From((Variant result, Variant error) =>
        {
            if (error.VariantType != Variant.Type.Nil)
            {
                callback(null, error.AsString());
                return;
            }

            if (result.VariantType == Variant.Type.Array)
            {
                var array = result.AsGodotArray();
                if (array.Count == 0)
                {
                    callback(null, null);
                    return;
                }
            }

            if (result.Obj is GodotObject obj)
            {
                callback(new StoreSubscriptionInfoStatus(obj), null);
            }
            else
            {
                callback(null, null);
            }
        });
        _instance.Call(new StringName("status_for_transaction"), transactionId, callable);
    }
}
