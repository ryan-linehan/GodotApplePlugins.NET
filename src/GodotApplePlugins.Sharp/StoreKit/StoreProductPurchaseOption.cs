using Godot;

namespace GodotApplePlugins.Sharp.StoreKit;

/// <summary>
/// C# wrapper for the StoreProductPurchaseOption GDExtension class.
/// Represents an option that can be supplied when purchasing a StoreProduct.
/// </summary>
public class StoreProductPurchaseOption
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new StoreProductPurchaseOption wrapper.
    /// </summary>
    public StoreProductPurchaseOption(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Creates a purchase option with an app account token (UUID string) that
    /// associates the transaction with a user account on your service.
    /// </summary>
    /// <param name="classInstance">The StoreProductPurchaseOption class instance.</param>
    /// <param name="uuidToken">The UUID string token.</param>
    /// <returns>A new purchase option.</returns>
    public static StoreProductPurchaseOption AppAccountToken(GodotObject classInstance, string uuidToken)
    {
        var result = classInstance.Call(new StringName("app_account_token"), uuidToken);
        return new StoreProductPurchaseOption((GodotObject)result.Obj!);
    }

    /// <summary>
    /// Creates a purchase option for introductory offer eligibility verification.
    /// </summary>
    /// <param name="classInstance">The StoreProductPurchaseOption class instance.</param>
    /// <param name="jws">The signed JWS string from the App Store.</param>
    /// <returns>A new purchase option.</returns>
    public static StoreProductPurchaseOption IntroductoryOfferEligibility(GodotObject classInstance, string jws)
    {
        var result = classInstance.Call(new StringName("introductory_offer_elligibility"), jws);
        return new StoreProductPurchaseOption((GodotObject)result.Obj!);
    }

    /// <summary>
    /// Creates a purchase option specifying the quantity of the product to purchase.
    /// </summary>
    /// <param name="classInstance">The StoreProductPurchaseOption class instance.</param>
    /// <param name="value">The quantity.</param>
    /// <returns>A new purchase option.</returns>
    public static StoreProductPurchaseOption Quantity(GodotObject classInstance, int value)
    {
        var result = classInstance.Call(new StringName("quantity"), value);
        return new StoreProductPurchaseOption((GodotObject)result.Obj!);
    }

    /// <summary>
    /// Creates a purchase option to simulate the 'Ask to Buy' flow in the sandbox environment.
    /// </summary>
    /// <param name="classInstance">The StoreProductPurchaseOption class instance.</param>
    /// <param name="enabled">Whether to enable the simulation.</param>
    /// <returns>A new purchase option.</returns>
    public static StoreProductPurchaseOption SimulateAskToBuyInSandbox(GodotObject classInstance, bool enabled)
    {
        var result = classInstance.Call(new StringName("simulate_ask_to_buy_in_sandbox"), enabled);
        return new StoreProductPurchaseOption((GodotObject)result.Obj!);
    }

    /// <summary>
    /// Creates a purchase option for a win-back offer.
    /// </summary>
    /// <param name="classInstance">The StoreProductPurchaseOption class instance.</param>
    /// <param name="offer">The subscription offer to use.</param>
    /// <returns>A new purchase option.</returns>
    public static StoreProductPurchaseOption WinBackOffer(GodotObject classInstance, StoreProductSubscriptionOffer offer)
    {
        var result = classInstance.Call(new StringName("win_back_offer"), offer.Instance);
        return new StoreProductPurchaseOption((GodotObject)result.Obj!);
    }
}
