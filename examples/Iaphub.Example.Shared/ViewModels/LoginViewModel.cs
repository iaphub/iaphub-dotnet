using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Iaphub.Example.Shared.Services;

namespace Iaphub.Example.Shared.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly MainViewModel _mainViewModel;
    private readonly IStoreStateService _storeStateService;

    [ObservableProperty]
    private string userId = "42";

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isModalVisible;

    public bool IsNotLoading => !IsLoading;
    public string LoginButtonText => IsLoading ? "Loading..." : "Login";

    public IAsyncRelayCommand ShowLoginModalCommand { get; }
    public IAsyncRelayCommand LoginCommand { get; }
    public IAsyncRelayCommand LoginAnonymouslyCommand { get; }
    public IRelayCommand CancelModalCommand { get; }

    public LoginViewModel(MainViewModel mainViewModel, IDialogService dialogService, IStoreStateService storeStateService)
    {
        _mainViewModel = mainViewModel;
        _dialogService = dialogService;
        _storeStateService = storeStateService;

        ShowLoginModalCommand = new AsyncRelayCommand(ShowLoginModalAsync, CanExecute);

        LoginCommand = new AsyncRelayCommand(LoginAsync, CanExecute);
        LoginAnonymouslyCommand = new AsyncRelayCommand(LoginAnonymouslyAsync, CanExecute);
        CancelModalCommand = new RelayCommand(
            () => IsModalVisible = false,
            CanExecute);
    }

    private async Task ShowLoginModalAsync()
    {
        var result = await _dialogService.ShowPromptAsync(
            "Login",
            "Enter your user ID (or leave as default for demo data):",
            "User ID",
            UserId
        );

        if (!string.IsNullOrWhiteSpace(result))
        {
            UserId = result;
            await LoginAsync();
        }
    }

    partial void OnIsLoadingChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotLoading));
        OnPropertyChanged(nameof(LoginButtonText));
        LoginCommand.NotifyCanExecuteChanged();
        LoginAnonymouslyCommand.NotifyCanExecuteChanged();
        CancelModalCommand.NotifyCanExecuteChanged();
    }

    private bool CanExecute() => !IsLoading;

    private async Task LoginAsync()
    {
        if (!CanExecute())
        {
            return;
        }

        IsLoading = true;
        try
        {
            var sanitizedUserId = string.IsNullOrWhiteSpace(UserId) ? "42" : UserId.Trim();

            await _storeStateService.LoginAsync(sanitizedUserId);

            IsModalVisible = false;
            _mainViewModel.NavigateToStore();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Error", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoginAnonymouslyAsync()
    {
        if (!CanExecute())
        {
            return;
        }

        IsLoading = true;
        try
        {
            await _storeStateService.LoginAnonymouslyAsync();
            IsModalVisible = false;
            _mainViewModel.NavigateToStore();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Error", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
