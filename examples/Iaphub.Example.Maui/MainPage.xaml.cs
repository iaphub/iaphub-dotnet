using System.ComponentModel;
using Iaphub.Example.Shared.ViewModels;
using Iaphub.Example.Maui.Views;

namespace Iaphub.Example.Maui;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to property changes
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.CurrentViewModel))
        {
            UpdateView();
        }
    }

    private void UpdateView()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_viewModel.CurrentViewModel is LoginViewModel loginViewModel)
            {
                var loginView = new LoginView
                {
                    BindingContext = loginViewModel
                };
                ContentArea.Content = loginView;
            }
            else if (_viewModel.CurrentViewModel is StoreViewModel storeViewModel)
            {
                var storeView = new Views.StoreView
                {
                    BindingContext = storeViewModel,
                    VerticalOptions = LayoutOptions.Fill
                };
                ContentArea.Content = storeView;
            }
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Set initial view after page appears
        UpdateView();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }
}
