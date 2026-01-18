using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.GameCenter;

/// <summary>
/// C# wrapper for the GKLocalPlayer GDExtension class.
/// Represents the authenticated local player on this device.
/// </summary>
public class GKLocalPlayer
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new GKLocalPlayer wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The GKLocalPlayer GDExtension object.</param>
    public GKLocalPlayer(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Reflects GKLocalPlayer.local.isAuthenticated.
    /// </summary>
    public bool IsAuthenticated => _instance.Get(ApplePluginStringNames.IsAuthenticated).AsBool();

    /// <summary>
    /// Apple's isUnderage flag for COPPA-compliant flows.
    /// </summary>
    public bool IsUnderage => _instance.Get(ApplePluginStringNames.IsUnderage).AsBool();

    /// <summary>
    /// Indicates if multiplayer gaming is restricted by parental controls.
    /// </summary>
    public bool IsMultiplayerGamingRestricted =>
        _instance.Get(ApplePluginStringNames.IsMultiplayerGamingRestricted).AsBool();

    /// <summary>
    /// Indicates if personalized communication is restricted.
    /// </summary>
    public bool IsPersonalizedCommunicationRestricted =>
        _instance.Get(ApplePluginStringNames.IsPersonalizedCommunicationRestricted).AsBool();

    /// <summary>
    /// Fetches the friends list.
    /// </summary>
    /// <param name="callback">
    /// Callback invoked with the list of friends and an optional error message.
    /// Error is null on success.
    /// </param>
    public void LoadFriends(Action<GKPlayer[], string?> callback)
    {
        var callable = Callable.From((Godot.Collections.Array friends, Variant error) =>
        {
            var players = friends
                .Select(f => new GKPlayer((GodotObject)f.Obj!))
                .ToArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(players, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.LoadFriends, callable);
    }

    /// <summary>
    /// Loads players who recently played with the local player.
    /// </summary>
    /// <param name="callback">
    /// Callback invoked with the list of recent players and an optional error message.
    /// </param>
    public void LoadRecentFriends(Action<GKPlayer[], string?> callback)
    {
        var callable = Callable.From((Godot.Collections.Array friends, Variant error) =>
        {
            var players = friends
                .Select(f => new GKPlayer((GodotObject)f.Obj!))
                .ToArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(players, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.LoadRecentFriends, callable);
    }

    /// <summary>
    /// Retrieves players the local user can challenge.
    /// </summary>
    /// <param name="callback">
    /// Callback invoked with the list of challengeable players and an optional error message.
    /// </param>
    public void LoadChallengeableFriends(Action<GKPlayer[], string?> callback)
    {
        var callable = Callable.From((Godot.Collections.Array friends, Variant error) =>
        {
            var players = friends
                .Select(f => new GKPlayer((GodotObject)f.Obj!))
                .ToArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(players, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.LoadChallengeableFriends, callable);
    }

    /// <summary>
    /// Retrieve the list of saved games.
    /// </summary>
    /// <param name="callback">
    /// Callback invoked with the list of saved games and an optional error message.
    /// </param>
    public void FetchSavedGames(Action<GKSavedGame[], string?> callback)
    {
        var callable = Callable.From((Godot.Collections.Array savedGames, Variant error) =>
        {
            var games = savedGames
                .Select(sg => new GKSavedGame((GodotObject)sg.Obj!))
                .ToArray();
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(games, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.FetchSavedGames, callable);
    }

    /// <summary>
    /// Stores a packed byte array as game data with the specified name.
    /// </summary>
    /// <param name="data">The game data to save.</param>
    /// <param name="name">The name for the saved game.</param>
    /// <param name="callback">
    /// Callback invoked with the saved game and an optional error message.
    /// </param>
    public void SaveGameData(byte[] data, string name, Action<GKSavedGame?, string?> callback)
    {
        var callable = Callable.From((Variant savedGame, Variant error) =>
        {
            GKSavedGame? game = null;
            if (savedGame.Obj is GodotObject obj)
            {
                game = new GKSavedGame(obj);
            }
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(game, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.SaveGameData, data, name, callable);
    }

    /// <summary>
    /// Removes saved game files by name.
    /// </summary>
    /// <param name="name">The name of the saved game to delete.</param>
    /// <param name="callback">
    /// Callback invoked with an optional error message. Null on success.
    /// </param>
    public void DeleteSavedGames(string name, Action<string?> callback)
    {
        var callable = Callable.From((Variant error) =>
        {
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(errorMsg);
        });
        _instance.Call(ApplePluginStringNames.DeleteSavedGames, name, callable);
    }

    /// <summary>
    /// Calls Apple's fetchItems helper for server-side authentication.
    /// </summary>
    /// <param name="callback">
    /// Callback invoked with the verification signature data and an optional error message.
    /// </param>
    public void FetchItemsForIdentityVerificationSignature(Action<IdentityVerificationSignature?, string?> callback)
    {
        var callable = Callable.From((Variant result, Variant error) =>
        {
            IdentityVerificationSignature? signature = null;
            if (result.VariantType != Variant.Type.Nil && result.Obj is Godot.Collections.Dictionary dict)
            {
                signature = new IdentityVerificationSignature
                {
                    PublicKeyUrl = dict.TryGetValue("public_key_url", out var url) ? url.AsString() : "",
                    Signature = dict.TryGetValue("signature", out var sig) ? sig.AsByteArray() : Array.Empty<byte>(),
                    Salt = dict.TryGetValue("salt", out var salt) ? salt.AsByteArray() : Array.Empty<byte>(),
                    Timestamp = dict.TryGetValue("timestamp", out var ts) ? ts.AsUInt64() : 0
                };
            }
            var errorMsg = error.VariantType == Variant.Type.Nil ? null : error.AsString();
            callback(signature, errorMsg);
        });
        _instance.Call(ApplePluginStringNames.FetchItemsForIdentityVerificationSignature, callable);
    }
}

/// <summary>
/// Data returned by FetchItemsForIdentityVerificationSignature for server-side verification.
/// </summary>
public class IdentityVerificationSignature
{
    /// <summary>The URL for the public key.</summary>
    public string PublicKeyUrl { get; set; } = "";

    /// <summary>The signature bytes.</summary>
    public byte[] Signature { get; set; } = Array.Empty<byte>();

    /// <summary>The salt bytes.</summary>
    public byte[] Salt { get; set; } = Array.Empty<byte>();

    /// <summary>The timestamp.</summary>
    public ulong Timestamp { get; set; }
}
