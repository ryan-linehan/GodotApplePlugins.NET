using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// C# wrapper for the StoreProduct GDExtension class.
/// Represents an in-app store product.
/// </summary>
public class StoreProduct
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new StoreProduct wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The StoreProduct GDExtension object.</param>
    public StoreProduct(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The product identifier configured in App Store Connect.
    /// </summary>
    public string ProductId => _instance.Get(ApplePluginStringNames.ProductId).AsString();

    /// <summary>
    /// The display name of the product.
    /// </summary>
    public string DisplayName => _instance.Get(ApplePluginStringNames.DisplayName).AsString();

    /// <summary>
    /// The product description.
    /// </summary>
    public string Description => _instance.Get(ApplePluginStringNames.DescriptionValue).AsString();

    /// <summary>
    /// The price as a numeric value.
    /// </summary>
    public double Price => _instance.Get(ApplePluginStringNames.Price).AsDouble();

    /// <summary>
    /// The formatted price string with currency symbol.
    /// </summary>
    public string DisplayPrice => _instance.Get(ApplePluginStringNames.DisplayPrice).AsString();

    /// <summary>
    /// Indicates if the product can be shared with family members.
    /// </summary>
    public bool IsFamilyShareable => _instance.Get(ApplePluginStringNames.IsFamilyShareable).AsBool();

    /// <summary>
    /// JSON representation of the product data.
    /// </summary>
    public string JsonRepresentation => _instance.Get(ApplePluginStringNames.JsonRepresentation).AsString();

    /// <summary>
    /// Returns a string representation of this product.
    /// </summary>
    public override string ToString()
    {
        return $"{DisplayName} ({ProductId}) - {DisplayPrice}";
    }
}
