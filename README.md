# IAPHUB - .NET In-App Purchase SDK

[![NuGet](https://img.shields.io/nuget/v/Iaphub.svg?label=NuGet)](https://www.nuget.org/packages/Iaphub/)

The easiest way to monetize your iOS and Android .NET apps by selling in-app purchases.

## Features

- 🚀 **Easy Integration** - Simple API for managing in-app purchases
- 📱 **Cross-Platform** - Works with .NET MAUI, Avalonia, and other .NET frameworks
- 🍎 **iOS Support** - Full StoreKit V1 and V2 integration
- 🤖 **Android Support** - Full Google Play Billing Library integration
- 💰 **Multiple Product Types** - Consumables, non-consumables, and subscriptions
- 🔄 **Receipt Validation** - Server-side receipt validation through IAPHUB
- 📊 **Analytics** - Built-in analytics and insights

## Supported Platforms

- **iOS**: iOS 13.0+
- **Android**: API Level 24+
- **Frameworks**: .NET MAUI, Avalonia, and other .NET 9+ frameworks

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package Iaphub
```

Or via Package Manager Console:

```powershell
Install-Package Iaphub
```

## Getting Started

Please refer to our [Quickstart Guide](https://www.iaphub.com/docs/getting-started/?sdk=dotnet) for step-by-step instructions on integrating IAPHUB into your .NET app.

You can also learn all about our SDK methods in our comprehensive [SDK Reference](https://www.iaphub.com/docs/sdk-reference/?sdk=dotnet).

## Quick Start

### Recommended Pattern: Service Wrapper

For better testability and state management, we recommend wrapping the SDK in a service class:

```csharp
using System.Collections.ObjectModel;

public interface IStoreService
{
    ObservableCollection<IaphubProduct> ProductsForSale { get; }
    ObservableCollection<IaphubActiveProduct> ActiveProducts { get; }

    Task InitializeAsync();
    Task<IaphubReceiptTransaction> BuyAsync(string sku);
    Task RestoreAsync();
    Task LoginAsync(string userId);
    Task LogoutAsync();
}

public class StoreService : IStoreService
{
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    public ObservableCollection<IaphubProduct> ProductsForSale { get; } = new();
    public ObservableCollection<IaphubActiveProduct> ActiveProducts { get; } = new();

    public async Task InitializeAsync()
    {
        await IaphubSdk.StartAsync(
            // Found in IAPHUB dashboard
            appId: "your-app-id",
            // Found in IAPHUB dashboard
            apiKey: "your-api-key",
            // Set to true to allow purchases without user login
            allowAnonymousPurchase: false,
            // Enable StoreKit V2 (recommended, requires App Store Server API configuration)
            enableStorekitV2: true
        );

        IaphubSdk.OnUserUpdate += async (s, e) => await RefreshProductsAsync();

        await RefreshProductsAsync();
    }

    private async Task RefreshProductsAsync()
    {
        if (!await _refreshLock.WaitAsync(0)) return;

        try
        {
            var products = await IaphubSdk.GetProductsAsync();

            ProductsForSale.Clear();
            foreach (var product in products.ProductsForSale)
                ProductsForSale.Add(product);

            ActiveProducts.Clear();
            foreach (var product in products.ActiveProducts)
                ActiveProducts.Add(product);
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    public Task<IaphubReceiptTransaction> BuyAsync(string sku) => IaphubSdk.BuyAsync(sku);
    public Task RestoreAsync() => IaphubSdk.RestoreAsync();
    public Task LoginAsync(string userId) => IaphubSdk.LoginAsync(userId);

    public async Task LogoutAsync()
    {
        await IaphubSdk.LogoutAsync();
        ProductsForSale.Clear();
        ActiveProducts.Clear();
    }
}
```

Then register it in your DI container:

```csharp
// MAUI (MauiProgram.cs)
builder.Services.AddSingleton<IStoreService, StoreService>();
```

Make sure to call `InitializeAsync()` when your app starts.

## Requirements

- .NET 9.0 or later
- An IAPHUB account (sign up at [iaphub.com](https://www.iaphub.com))

## Support

- 📧 Email: support@iaphub.com
- 🐛 Issues: https://github.com/iaphub/dotnet-iaphub/issues

## License

This project is licensed under the MIT License.