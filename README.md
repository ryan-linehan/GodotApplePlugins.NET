# GodotApplePlugins.Sharp

A C# wrapper for [GodotApplePlugins](https://github.com/migueldeicaza/GodotApplePlugins) - provides typed access to Apple APIs (GameCenter, StoreKit, Sign in with Apple) in Godot C# projects.

## Installation

### NuGet Package

```bash
dotnet add package GodotApplePlugins.Sharp
```

Or add to your `.csproj`:

```xml
<PackageReference Include="GodotApplePlugins.Sharp" Version="0.0.1-alpha" />
```

### Requirements

- Godot 4.3+
- .NET 8.0
- [GodotApplePlugins](https://github.com/migueldeicaza/GodotApplePlugins) GDExtension installed in your project
- macOS 14.0+ or iOS 17.0+

## Usage

### Game Center

```csharp
using GodotApplePlugins.Sharp;
using GodotApplePlugins.Sharp.GameCenter;

public partial class MyGame : Node
{
    private GameCenterManager? _gameCenter;

    public override void _Ready()
    {
        // Check if running on Apple platform
        if (!ApplePlugins.IsAvailable())
        {
            GD.Print("Apple plugins not available on this platform");
            return;
        }

        // Create the manager
        _gameCenter = ApplePlugins.TryCreateGameCenterManager();
        if (_gameCenter == null)
        {
            GD.Print("GameCenterManager not found - is the GDExtension installed?");
            return;
        }

        // Subscribe to events
        _gameCenter.AuthenticationResult += OnAuthResult;
        _gameCenter.AuthenticationError += OnAuthError;

        // Authenticate
        _gameCenter.Authenticate();
    }

    private void OnAuthResult(bool success)
    {
        if (success)
        {
            GD.Print("Game Center authenticated!");

            // Access the local player
            var localPlayer = _gameCenter!.LocalPlayer;
            GD.Print($"Player authenticated: {localPlayer?.IsAuthenticated}");

            // Show the Game Center access point
            var accessPoint = _gameCenter.AccessPoint;
            if (accessPoint != null)
            {
                accessPoint.Location = GKAccessPointLocation.TopTrailing;
                accessPoint.ShowHighlights = true;
                accessPoint.Active = true;
            }
        }
    }

    private void OnAuthError(string message)
    {
        GD.PrintErr($"Game Center auth failed: {message}");
    }
}
```

### Achievements

```csharp
// Report achievement progress
var achievement = new GKAchievement(achievementInstance);
achievement.Identifier = "com.yourapp.achievement1";
achievement.PercentComplete = 100.0;
achievement.ShowsCompletionBanner = true;

GKAchievement.ReportAchievements(achievementClass, new[] { achievement }, error =>
{
    if (error == null)
        GD.Print("Achievement reported!");
    else
        GD.PrintErr($"Failed to report achievement: {error}");
});
```

### Leaderboards

```csharp
// Submit a score
leaderboard.SubmitScore(1000, 0, localPlayer, error =>
{
    if (error == null)
        GD.Print("Score submitted!");
});

// Load leaderboard entries
leaderboard.LoadLocalPlayerEntries(
    GKLeaderboardPlayerScope.Global,
    GKLeaderboardTimeScope.AllTime,
    1, 10,
    (entries, localEntry, total, error) =>
    {
        if (error == null)
        {
            foreach (var entry in entries)
            {
                GD.Print($"Rank {entry.Rank}: {entry.Player.DisplayName} - {entry.FormattedScore}");
            }
        }
    });
```

### StoreKit (In-App Purchases)

```csharp
using GodotApplePlugins.Sharp.StoreKit;

private StoreKitManager? _storeKit;

public void InitializeStore()
{
    _storeKit = ApplePlugins.TryCreateStoreKitManager();
    if (_storeKit == null) return;

    // Subscribe to events
    _storeKit.ProductsRequestCompleted += OnProductsLoaded;
    _storeKit.PurchaseCompleted += OnPurchaseCompleted;
    _storeKit.RestoreCompleted += OnRestoreCompleted;

    // Request products
    _storeKit.RequestProducts(new[] { "com.yourapp.product1", "com.yourapp.premium" });
}

private void OnProductsLoaded(StoreProduct[] products, StoreKitStatus status)
{
    if (status == StoreKitStatus.Success)
    {
        foreach (var product in products)
        {
            GD.Print($"Product: {product.DisplayName} - {product.DisplayPrice}");
        }
    }
}

private void OnPurchaseCompleted(StoreTransaction? transaction, StoreKitStatus status, string message)
{
    if (status == StoreKitStatus.Success && transaction != null)
    {
        GD.Print($"Purchase successful: {transaction.ProductId}");
        // Unlock content, save to server, etc.
    }
    else if (status == StoreKitStatus.UserCancelled)
    {
        GD.Print("User cancelled purchase");
    }
    else
    {
        GD.PrintErr($"Purchase failed: {message}");
    }
}

// Purchase a product
public void PurchaseProduct(StoreProduct product)
{
    _storeKit?.Purchase(product);
}

// Restore purchases
public void RestorePurchases()
{
    _storeKit?.RestorePurchases();
}
```

### Sign in with Apple

```csharp
using GodotApplePlugins.Sharp.Authentication;

private ASAuthorizationController? _authController;

public void InitializeSignIn()
{
    _authController = ApplePlugins.TryCreateAuthorizationController();
    if (_authController == null) return;

    _authController.AuthorizationCompleted += OnSignInSuccess;
    _authController.AuthorizationFailed += OnSignInFailed;
}

public void SignInWithApple()
{
    // Request email and full name
    _authController?.SignInWithEmailAndFullName();

    // Or use custom scopes
    // _authController?.SignInWithScopes(new[] { ASAuthorizationScopes.Email });
}

private void OnSignInSuccess(AppleIdCredential credential)
{
    GD.Print($"Signed in as: {credential.User}");

    if (credential.Email != null)
        GD.Print($"Email: {credential.Email}");

    if (credential.FullName != null)
        GD.Print($"Name: {credential.FullName.FormattedName}");

    // Send identity token to your server for validation
    if (credential.IdentityToken != null)
    {
        // ValidateWithServer(credential.IdentityToken);
    }
}

private void OnSignInFailed(string message)
{
    GD.PrintErr($"Sign in failed: {message}");
}
```

## API Reference

### Namespaces

- `GodotApplePlugins.Sharp` - Main entry point and utilities
- `GodotApplePlugins.Sharp.GameCenter` - Game Center APIs
- `GodotApplePlugins.Sharp.StoreKit` - In-app purchase APIs
- `GodotApplePlugins.Sharp.Authentication` - Sign in with Apple
- `GodotApplePlugins.Sharp.Shared` - Enums and shared utilities

### Key Classes

| Class | Description |
|-------|-------------|
| `ApplePlugins` | Static helper for creating manager instances |
| `GameCenterManager` | Entry point for Game Center authentication |
| `GKLocalPlayer` | The authenticated local player |
| `GKAccessPoint` | Game Center UI access point |
| `GKAchievement` | Achievement tracking |
| `GKLeaderboard` | Leaderboard management |
| `StoreKitManager` | In-app purchase management |
| `StoreProduct` | Product information |
| `StoreTransaction` | Purchase transaction |
| `ASAuthorizationController` | Sign in with Apple |
| `AppleIdCredential` | Apple ID credential data |

## License

MIT

## Credits

This wrapper is built on top of [GodotApplePlugins](https://github.com/migueldeicaza/GodotApplePlugins) by Miguel de Icaza.
