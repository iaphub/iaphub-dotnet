using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Iaphub.Example.Shared.Services;

namespace Iaphub.Example.Shared.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogService _dialogService;
    private readonly IStoreStateService _storeStateService;

    [ObservableProperty]
    private ViewModelBase? currentViewModel;

    [ObservableProperty]
    private bool isLoggedIn;

    public MainViewModel(IServiceProvider serviceProvider, IDialogService dialogService, IStoreStateService storeStateService)
    {
        _serviceProvider = serviceProvider;
        _dialogService = dialogService;
        _storeStateService = storeStateService;
        // DON'T call NavigateToLogin() here - it causes circular dependency with LoginViewModel
        // Call Initialize() after construction is complete
    }

    public void Initialize()
    {
        _ = InitializeAsync();
        NavigateToLogin();
    }

    private async Task InitializeAsync()
    {
        try
        {
            await _storeStateService.InitializeSdkAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("IAPHUB", $"Initialization failed: {ex.Message}");
        }
    }

    public void NavigateToLogin()
    {
        IsLoggedIn = false;
        SwitchViewModel(_serviceProvider.GetRequiredService<LoginViewModel>());
    }

    public void NavigateToStore()
    {
        IsLoggedIn = true;
        SwitchViewModel(_serviceProvider.GetRequiredService<StoreViewModel>());
    }

    private void SwitchViewModel(ViewModelBase next)
    {
        if (ReferenceEquals(CurrentViewModel, next))
        {
            return;
        }

        (CurrentViewModel as IDisposable)?.Dispose();
        CurrentViewModel = next;
    }

    public override void Dispose()
    {
        base.Dispose();
        try
        {
            (CurrentViewModel as IDisposable)?.Dispose();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dispose failed: {ex}");
        }
    }
}
