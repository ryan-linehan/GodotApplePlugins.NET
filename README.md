# GodotApplePlugins.NET

A C# wrapper for [GodotApplePlugins](https://github.com/migueldeicaza/GodotApplePlugins) - provides typed access to Apple APIs (GameCenter, StoreKit, Sign in with Apple, AVFoundation, and more) in Godot C# projects.

## Features

- **Generated from source** - Wrappers are automatically generated from GodotApplePlugins XML documentation
- **Type-safe** - Compile-time checking for method names, parameters, and return types
- **C# events** - Godot signals exposed as native C# events
- **Full coverage** - All GodotApplePlugins classes wrapped

## Installation

### NuGet Package

```bash
dotnet add package GodotApplePlugins.NET
```

Or add to your `.csproj`:

```xml
<PackageReference Include="GodotApplePlugins.NET" Version="0.0.1-alpha" />
```

### Requirements

- Godot 4.3+
- .NET 8.0
- [GodotApplePlugins](https://github.com/migueldeicaza/GodotApplePlugins) GDExtension installed in your project
- macOS 14.0+ or iOS 17.0+

## Usage

### Game Center

```csharp
using GodotApplePlugins.NET;
using GodotApplePlugins.NET.GameCenter;

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
        }
    }

    private void OnAuthError(string message)
    {
        GD.PrintErr($"Game Center auth failed: {message}");
    }
}
```

### StoreKit (In-App Purchases)

```csharp
using GodotApplePlugins.NET;
using GodotApplePlugins.NET.StoreKit;

private StoreKitManager? _storeKit;

public void InitializeStore()
{
    _storeKit = ApplePlugins.TryCreateStoreKitManager();
    if (_storeKit == null) return;

    // Subscribe to events
    _storeKit.ProductsRequestCompleted += OnProductsLoaded;
    _storeKit.PurchaseCompleted += OnPurchaseCompleted;

    // Request products
    _storeKit.RequestProducts(new[] { "com.yourapp.product1" });
}

private void OnProductsLoaded(StoreProduct[] products, int status)
{
    foreach (var product in products)
    {
        GD.Print($"Product: {product.DisplayName} - {product.DisplayPrice}");
    }
}

private void OnPurchaseCompleted(StoreTransaction transaction, int status, string message)
{
    if (status == 0) // OK
    {
        GD.Print($"Purchase successful: {transaction.ProductId}");
        transaction.Finish(); // Important: call finish after delivering content
    }
}
```

### Sign in with Apple

```csharp
using GodotApplePlugins.NET;
using GodotApplePlugins.NET.Authentication;

var authController = ApplePlugins.TryCreateASAuthorizationController();
if (authController != null)
{
    authController.AuthorizationCompleted += credential =>
    {
        GD.Print($"Signed in as: {credential.User}");
    };

    authController.AuthorizationFailed += error =>
    {
        GD.PrintErr($"Sign in failed: {error}");
    };

    authController.Signin();
}
```

## API Reference

### Namespaces

| Namespace | Description |
|-----------|-------------|
| `GodotApplePlugins.NET` | Main entry point (`ApplePlugins` class) |
| `GodotApplePlugins.NET.GameCenter` | Game Center APIs |
| `GodotApplePlugins.NET.StoreKit` | In-app purchase APIs |
| `GodotApplePlugins.NET.Authentication` | Sign in with Apple, OAuth |
| `GodotApplePlugins.NET.AVFoundation` | Audio session control |
| `GodotApplePlugins.NET.Foundation` | Foundation utilities (URL, UUID) |
| `GodotApplePlugins.NET.UI` | Native UI (file picker) |

### Key Classes

| Class | Description |
|-------|-------------|
| `ApplePlugins` | Static factory for creating wrapper instances |
| `GameCenterManager` | Entry point for Game Center authentication |
| `GKLocalPlayer` | The authenticated local player |
| `GKMatch` | Real-time multiplayer match |
| `StoreKitManager` | In-app purchase management |
| `StoreProduct` | Product information |
| `StoreTransaction` | Purchase transaction |
| `ASAuthorizationController` | Sign in with Apple |
| `ASWebAuthenticationSession` | OAuth/web authentication |
| `AVAudioSession` | iOS audio session control |
| `AppleFilePicker` | Native file picker |

## Development

### Regenerating Wrappers

The C# wrappers are generated from GodotApplePlugins XML documentation. To regenerate:

1. Update the XML files in `doc_classes/` from upstream
2. Run the generator:

```bash
./generate.sh
```

Or manually:

```bash
dotnet run --project src/GodotApplePlugins.Generator -- doc_classes src/GodotApplePlugins.NET/Generated
```

### Project Structure

```
GodotApplePlugins.NET/
├── doc_classes/                    # XML docs from GodotApplePlugins
├── src/
│   ├── GodotApplePlugins.NET/   # The NuGet package
│   │   └── Generated/             # Auto-generated wrappers
│   └── GodotApplePlugins.Generator/ # Code generator
├── generate.sh                     # Regeneration script
└── README.md
```

## License

MIT

## Credits

This wrapper is built on top of [GodotApplePlugins](https://github.com/migueldeicaza/GodotApplePlugins) by Miguel de Icaza.
