using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// C# wrapper for the StoreTransaction GDExtension class.
/// Represents a completed or pending transaction.
/// </summary>
public class StoreTransaction
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new StoreTransaction wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The StoreTransaction GDExtension object.</param>
    public StoreTransaction(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The unique transaction identifier.
    /// </summary>
    public string TransactionId => _instance.Get(ApplePluginStringNames.TransactionId).AsString();

    /// <summary>
    /// The original transaction identifier (for renewals/restores).
    /// </summary>
    public string OriginalTransactionId => _instance.Get(ApplePluginStringNames.OriginalTransactionId).AsString();

    /// <summary>
    /// The product identifier for this transaction.
    /// </summary>
    public string ProductId => _instance.Get(ApplePluginStringNames.ProductId).AsString();

    /// <summary>
    /// The purchase date (Unix timestamp).
    /// </summary>
    public double PurchaseDate => _instance.Get(ApplePluginStringNames.PurchaseDate).AsDouble();

    /// <summary>
    /// The expiration date for subscriptions (Unix timestamp).
    /// Returns 0 for non-subscription products.
    /// </summary>
    public double ExpirationDate => _instance.Get(ApplePluginStringNames.ExpirationDate).AsDouble();

    /// <summary>
    /// Gets the purchase date as a DateTime.
    /// </summary>
    public DateTime PurchaseDateTime => DateTimeOffset.FromUnixTimeSeconds((long)PurchaseDate).DateTime;

    /// <summary>
    /// Gets the expiration date as a DateTime, or null for non-subscriptions.
    /// </summary>
    public DateTime? ExpirationDateTime
    {
        get
        {
            var exp = ExpirationDate;
            return exp > 0 ? DateTimeOffset.FromUnixTimeSeconds((long)exp).DateTime : null;
        }
    }

    /// <summary>
    /// Returns a string representation of this transaction.
    /// </summary>
    public override string ToString()
    {
        return $"Transaction {TransactionId} for {ProductId}";
    }
}
