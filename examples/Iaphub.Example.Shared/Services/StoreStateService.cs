using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using global::Iaphub;
using Iaphub.Sdk;

namespace Iaphub.Example.Shared.Services;

public interface IStoreStateService : IDisposable, INotifyPropertyChanged
{
    ObservableCollection<IaphubProduct> ProductsForSale { get; }
    ObservableCollection<IaphubActiveProduct> ActiveProducts { get; }
    bool IsInitialized { get; }
    bool IsLoading { get; }
    Task RefreshProductsAsync();
    Task InitializeSdkAsync();
    Task LoginAsync(string userId);
    Task LoginAnonymouslyAsync();
    Task<IaphubReceiptTransaction> BuyAsync(string sku);
    Task RestoreAsync();
    Task LogoutAsync();
    Task ShowManageSubscriptionsAsync();
    Task PresentCodeRedemptionSheetAsync();
}

public class StoreStateService : ObservableObject, IStoreStateService
{
    private readonly IDialogService _dialogService;
    private readonly SemaphoreSlim _loadSemaphore = new(1, 1);
    private bool _isInitialized;
    private bool _eventsRegistered;
    private bool _sdkInitialized;

    public ObservableCollection<IaphubProduct> ProductsForSale { get; } = new();
    public ObservableCollection<IaphubActiveProduct> ActiveProducts { get; } = new();

    public bool IsInitialized
    {
        get => _isInitialized;
        private set
        {
            if (SetProperty(ref _isInitialized, value))
            {
                OnPropertyChanged(nameof(IsLoading));
            }
        }
    }

    public bool IsLoading => !IsInitialized;

    public StoreStateService(IDialogService dialogService)
    {
        _dialogService = dialogService;
        RegisterEventHandlers();
    }

    public async Task RefreshProductsAsync()
    {
        await EnsureInitializedAsync();
        RegisterEventHandlers();

        // Skip if a refresh is already in progress
        if (!await _loadSemaphore.WaitAsync(0))
        {
            return;
        }

        try
        {
            IsInitialized = false;

            var products = await IaphubSdk.GetProductsAsync();

            var productsForSale = products.ProductsForSale;
            var activeProducts = products.ActiveProducts;

            ProductsForSale.Clear();
            foreach (var product in productsForSale)
            {
                ProductsForSale.Add(product);
            }

            ActiveProducts.Clear();
            foreach (var product in activeProducts)
            {
                ActiveProducts.Add(product);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Error", $"Failed to load products: {ex.Message}");
        }
        finally
        {
            IsInitialized = true;
            _loadSemaphore.Release();
        }
    }

    public async Task InitializeSdkAsync()
    {
        if (_sdkInitialized)
        {
            return;
        }

        try
        {
            await IaphubSdk.StartAsync(
                appId: "5e4890f6c61fc971cf46db4d",
                apiKey: "SDp7aY220RtzZrsvRpp4BGFm6qZqNkNf",
                allowAnonymousPurchase: true,
                enableStorekitV2: true,
                lang: "en"
            ).ConfigureAwait(false);

            _sdkInitialized = true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task LoginAsync(string userId)
    {
        await EnsureInitializedAsync().ConfigureAwait(false);
        await IaphubSdk.LoginAsync(userId).ConfigureAwait(false);
    }

    public async Task LoginAnonymouslyAsync()
    {
        await RefreshProductsAsync();
    }

    public async Task<IaphubReceiptTransaction> BuyAsync(string sku)
    {
        await EnsureInitializedAsync().ConfigureAwait(false);
        return await IaphubSdk.BuyAsync(sku).ConfigureAwait(false);
    }

    public async Task RestoreAsync()
    {
        await EnsureInitializedAsync().ConfigureAwait(false);
        await IaphubSdk.RestoreAsync().ConfigureAwait(false);
    }

    public async Task LogoutAsync()
    {
        await EnsureInitializedAsync().ConfigureAwait(false);
        await IaphubSdk.LogoutAsync().ConfigureAwait(false);
        ResetState();
    }

    public async Task ShowManageSubscriptionsAsync()
    {
        await EnsureInitializedAsync().ConfigureAwait(false);
        await IaphubSdk.ShowManageSubscriptionsAsync().ConfigureAwait(false);
    }

    public async Task PresentCodeRedemptionSheetAsync()
    {
        await EnsureInitializedAsync().ConfigureAwait(false);
        await IaphubSdk.PresentCodeRedemptionSheetAsync().ConfigureAwait(false);
    }

    public void Dispose()
    {
        UnregisterEventHandlers();
        _loadSemaphore.Dispose();
    }

    private void RegisterEventHandlers()
    {
        if (_eventsRegistered)
        {
            return;
        }

        IaphubSdk.OnUserUpdate += HandleUserUpdate;
        IaphubSdk.OnDeferredPurchase += HandleDeferredPurchase;
        IaphubSdk.OnError += HandleError;
        IaphubSdk.OnBuyRequest += HandleBuyRequest;

        _eventsRegistered = true;
    }

    private void UnregisterEventHandlers()
    {
        if (!_eventsRegistered)
        {
            return;
        }

        IaphubSdk.OnUserUpdate -= HandleUserUpdate;
        IaphubSdk.OnDeferredPurchase -= HandleDeferredPurchase;
        IaphubSdk.OnError -= HandleError;
        IaphubSdk.OnBuyRequest -= HandleBuyRequest;

        _eventsRegistered = false;
    }

    private void HandleUserUpdate(object? sender, EventArgs e)
    {
        _ = RefreshProductsAsync();
    }

    private void HandleDeferredPurchase(object? sender, IaphubReceiptTransaction transaction)
    {
        Console.WriteLine($"-> Got deferred purchase: {transaction.Sku ?? transaction.Id}");
    }

    private void HandleError(object? sender, IaphubError error)
    {
        Console.WriteLine($"-> Got error: {error.Code} - {error.Message}");
    }

    private void HandleBuyRequest(object? sender, string sku)
    {
        Console.WriteLine($"-> Got buy request: {sku}");
    }

    private async Task EnsureInitializedAsync()
    {
        if (_sdkInitialized)
        {
            return;
        }

        await InitializeSdkAsync().ConfigureAwait(false);
    }

    private void ResetState()
    {
        ProductsForSale.Clear();
        ActiveProducts.Clear();
        IsInitialized = false;
    }
}
