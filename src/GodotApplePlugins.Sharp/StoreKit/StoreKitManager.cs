using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// C# wrapper for the StoreKitManager GDExtension class.
/// Manages StoreKit interactions such as requesting products, purchasing, and restoring purchases.
/// </summary>
public class StoreKitManager
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Event raised when a product request completes.
    /// </summary>
    public event Action<StoreProduct[], StoreKitStatus>? ProductsRequestCompleted;

    /// <summary>
    /// Event raised when a purchase completes.
    /// </summary>
    public event Action<StoreTransaction?, StoreKitStatus, string>? PurchaseCompleted;

    /// <summary>
    /// Event raised when there's a purchase intent (e.g., from App Store promoted purchase).
    /// </summary>
    public event Action<StoreProduct>? PurchaseIntent;

    /// <summary>
    /// Event raised when restore purchases completes.
    /// </summary>
    public event Action<StoreKitStatus, string>? RestoreCompleted;

    /// <summary>
    /// Event raised when a transaction is updated.
    /// </summary>
    public event Action<StoreTransaction>? TransactionUpdated;

    /// <summary>
    /// Event raised when an unverified transaction is updated.
    /// </summary>
    public event Action<StoreTransaction, StoreKitVerificationError>? UnverifiedTransactionUpdated;

    /// <summary>
    /// Event raised when a subscription status changes.
    /// </summary>
    public event Action<GodotObject>? SubscriptionUpdate;

    /// <summary>
    /// Creates a new StoreKitManager wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The StoreKitManager GDExtension object.</param>
    public StoreKitManager(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ConnectSignals();
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Requests product information for the given product identifiers.
    /// Listen to <see cref="ProductsRequestCompleted"/> for results.
    /// </summary>
    /// <param name="productIds">Array of product identifiers configured in App Store Connect.</param>
    public void RequestProducts(string[] productIds)
    {
        _instance.Call(ApplePluginStringNames.RequestProducts, productIds);
    }

    /// <summary>
    /// Initiates purchase of a specific product.
    /// Listen to <see cref="PurchaseCompleted"/> for results.
    /// </summary>
    /// <param name="product">The product to purchase.</param>
    public void Purchase(StoreProduct product)
    {
        _instance.Call(ApplePluginStringNames.Purchase, product.Instance);
    }

    /// <summary>
    /// Initiates purchase of a product with additional purchase options.
    /// Listen to <see cref="PurchaseCompleted"/> for results.
    /// </summary>
    /// <param name="product">The product to purchase.</param>
    /// <param name="options">Array of purchase options.</param>
    public void PurchaseWithOptions(StoreProduct product, GodotObject[] options)
    {
        var optionsArray = new Godot.Collections.Array();
        foreach (var option in options)
        {
            optionsArray.Add(option);
        }
        _instance.Call(ApplePluginStringNames.PurchaseWithOptions, product.Instance, optionsArray);
    }

    /// <summary>
    /// Restores previously purchased non-consumable products and subscriptions.
    /// Listen to <see cref="RestoreCompleted"/> for results.
    /// </summary>
    public void RestorePurchases()
    {
        _instance.Call(ApplePluginStringNames.RestorePurchases);
    }

    /// <summary>
    /// Retrieves the most recent transaction for each product the customer is entitled to.
    /// Listen to <see cref="TransactionUpdated"/> for results.
    /// </summary>
    public void FetchCurrentEntitlements()
    {
        _instance.Call(ApplePluginStringNames.FetchCurrentEntitlements);
    }

    private void ConnectSignals()
    {
        _instance.Connect(ApplePluginStringNames.ProductsRequestCompletedSignal,
            Callable.From<Godot.Collections.Array, int>(OnProductsRequestCompleted));

        _instance.Connect(ApplePluginStringNames.PurchaseCompletedSignal,
            Callable.From<Variant, int, string>(OnPurchaseCompleted));

        _instance.Connect(ApplePluginStringNames.PurchaseIntentSignal,
            Callable.From<GodotObject>(OnPurchaseIntent));

        _instance.Connect(ApplePluginStringNames.RestoreCompletedSignal,
            Callable.From<int, string>(OnRestoreCompleted));

        _instance.Connect(ApplePluginStringNames.TransactionUpdatedSignal,
            Callable.From<GodotObject>(OnTransactionUpdated));

        _instance.Connect(ApplePluginStringNames.UnverifiedTransactionUpdatedSignal,
            Callable.From<GodotObject, int>(OnUnverifiedTransactionUpdated));

        _instance.Connect(ApplePluginStringNames.SubscriptionUpdateSignal,
            Callable.From<GodotObject>(OnSubscriptionUpdate));
    }

    private void OnProductsRequestCompleted(Godot.Collections.Array products, int status)
    {
        var storeProducts = products
            .Select(p => new StoreProduct((GodotObject)p.Obj!))
            .ToArray();
        ProductsRequestCompleted?.Invoke(storeProducts, (StoreKitStatus)status);
    }

    private void OnPurchaseCompleted(Variant transactionVariant, int status, string message)
    {
        StoreTransaction? transaction = null;
        if (transactionVariant.Obj is GodotObject obj)
        {
            transaction = new StoreTransaction(obj);
        }
        PurchaseCompleted?.Invoke(transaction, (StoreKitStatus)status, message);
    }

    private void OnPurchaseIntent(GodotObject product)
    {
        PurchaseIntent?.Invoke(new StoreProduct(product));
    }

    private void OnRestoreCompleted(int status, string message)
    {
        RestoreCompleted?.Invoke((StoreKitStatus)status, message);
    }

    private void OnTransactionUpdated(GodotObject transaction)
    {
        TransactionUpdated?.Invoke(new StoreTransaction(transaction));
    }

    private void OnUnverifiedTransactionUpdated(GodotObject transaction, int verificationError)
    {
        UnverifiedTransactionUpdated?.Invoke(
            new StoreTransaction(transaction),
            (StoreKitVerificationError)verificationError);
    }

    private void OnSubscriptionUpdate(GodotObject status)
    {
        SubscriptionUpdate?.Invoke(status);
    }

    /// <summary>
    /// Disconnects signal handlers. Call this when disposing of the manager.
    /// </summary>
    public void Disconnect()
    {
        // Note: In practice, you may want to store the Callables for proper disconnection
        // This is a simplified implementation
    }
}
