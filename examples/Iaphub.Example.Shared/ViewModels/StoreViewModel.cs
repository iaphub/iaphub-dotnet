using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using global::Iaphub;
using Iaphub.Example.Shared.Models;
using Iaphub.Example.Shared.Services;
using Iaphub.Sdk;

namespace Iaphub.Example.Shared.ViewModels;

public partial class StoreViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly MainViewModel _mainViewModel;
    private readonly IStoreStateService _storeStateService;
    private bool _disposed;

    public ObservableCollection<ProductViewModel> ProductsForSale { get; } = new();
    public ObservableCollection<IaphubActiveProduct> ActiveProducts => _storeStateService.ActiveProducts;
    public bool IsInitialized => _storeStateService.IsInitialized;
    public bool IsLoading => _storeStateService.IsLoading;
    public bool IsIos { get; } = OperatingSystem.IsIOS();
    
    public bool HasProductsForSale => ProductsForSale.Count > 0;
    public bool HasNoProductsForSale => ProductsForSale.Count == 0;
    public bool HasActiveProducts => ActiveProducts.Count > 0;
    public bool HasNoActiveProducts => ActiveProducts.Count == 0;

    public IAsyncRelayCommand<string?> BuyCommand { get; }
    public IAsyncRelayCommand RestoreCommand { get; }
    public IAsyncRelayCommand ShowManageSubscriptionsCommand { get; }
    public IAsyncRelayCommand PresentCodeRedemptionCommand { get; }
    public IAsyncRelayCommand LogoutCommand { get; }

    public StoreViewModel(MainViewModel mainViewModel, IDialogService dialogService, IStoreStateService storeStateService)
    {
        _mainViewModel = mainViewModel;
        _dialogService = dialogService;
        _storeStateService = storeStateService;

        BuyCommand = new AsyncRelayCommand<string?>(BuyAsync);
        RestoreCommand = new AsyncRelayCommand(RestoreAsync);
        ShowManageSubscriptionsCommand = new AsyncRelayCommand(() => _storeStateService.ShowManageSubscriptionsAsync());
        PresentCodeRedemptionCommand = new AsyncRelayCommand(() => _storeStateService.PresentCodeRedemptionSheetAsync());
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);

        _storeStateService.PropertyChanged += HandleStoreStateChanged;

        // Subscribe to collection changes to update Has* properties
        _storeStateService.ProductsForSale.CollectionChanged += (s, e) =>
        {
            UpdateProductViewModels();
            OnPropertyChanged(nameof(HasProductsForSale));
            OnPropertyChanged(nameof(HasNoProductsForSale));
        };
        ActiveProducts.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(HasActiveProducts));
            OnPropertyChanged(nameof(HasNoActiveProducts));
        };

        _ = _storeStateService.RefreshProductsAsync();
    }

    private void UpdateProductViewModels()
    {
        ProductsForSale.Clear();
        foreach (var product in _storeStateService.ProductsForSale)
        {
            ProductsForSale.Add(new ProductViewModel { Product = product });
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_disposed)
        {
            return;
        }

        _storeStateService.PropertyChanged -= HandleStoreStateChanged;
        _disposed = true;
    }

    private async Task BuyAsync(string? sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            return;
        }

        var productVm = ProductsForSale.FirstOrDefault(p => p.Product.Sku == sku);
        if (productVm == null) return;

        productVm.IsProcessing = true;
        try
        {
            var transaction = await _storeStateService.BuyAsync(sku);

            if (string.Equals(transaction.WebhookStatus, "failed", StringComparison.OrdinalIgnoreCase))
            {
                await _dialogService.ShowAlertAsync(
                    "Purchase delayed",
                    "Your purchase was successful but we need some more time to validate it.");
            }
            else
            {
                await _dialogService.ShowAlertAsync(
                    "Purchase successful",
                    "Your purchase has been processed successfully!");
            }
        }
        catch (Exception ex)
        {
            await HandlePurchaseError(ex);
        }
        finally
        {
            productVm.IsProcessing = false;
        }
    }

    private Task HandlePurchaseError(Exception ex)
    {
        // Don't show alert if user cancelled the purchase
        if (ex is IaphubException iaphubEx && iaphubEx.Code == "user_cancelled")
        {
            return Task.CompletedTask;
        }

        var fallback = "Unable to process purchase, try again later.";
        var message = string.IsNullOrWhiteSpace(ex.Message) ? fallback : ex.Message;
        return _dialogService.ShowAlertAsync("Error", message);
    }

    private async Task RestoreAsync()
    {
        try
        {
            await _storeStateService.RestoreAsync();
            await _dialogService.ShowAlertAsync("Restore", "Purchases restored successfully!");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Error", ex.Message);
        }
    }

    private async Task LogoutAsync()
    {
        await _storeStateService.LogoutAsync();
        _mainViewModel.NavigateToLogin();
    }

    private void HandleStoreStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.PropertyName)
            || e.PropertyName == nameof(IStoreStateService.IsInitialized))
        {
            OnPropertyChanged(nameof(IsInitialized));
            OnPropertyChanged(nameof(IsLoading));
        }
    }
}
