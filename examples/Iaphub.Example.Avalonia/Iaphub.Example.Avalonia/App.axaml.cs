using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Iaphub.Example.Shared.Services;
using Iaphub.Example.Shared.ViewModels;
using Iaphub.Example.Avalonia.Views;

namespace Iaphub.Example.Avalonia;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ConfigureServices();

        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var mainViewModel = _serviceProvider!.GetRequiredService<MainViewModel>();
            mainViewModel.Initialize();

            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<IStoreStateService, StoreStateService>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<StoreViewModel>();

        // Use runtime platform detection instead of compile-time #if directives
        if (OperatingSystem.IsIOS())
        {
            // Try both assembly names (regular and Nuget examples)
            var iosDialogServiceType = Type.GetType("Iaphub.Example.Avalonia.iOS.Services.IOSDialogService, Iaphub.Example.Avalonia.iOS")
                ?? Type.GetType("Iaphub.Example.Avalonia.Nuget.iOS.Services.IOSDialogService, Iaphub.Example.Avalonia.Nuget.iOS");

            if (iosDialogServiceType != null)
            {
                services.AddSingleton(typeof(IDialogService), iosDialogServiceType);
            }
        }
        else if (OperatingSystem.IsAndroid())
        {
            // Try both assembly names (regular and Nuget examples)
            var androidDialogServiceType = Type.GetType("Iaphub.Example.Avalonia.Android.Services.AndroidDialogService, Iaphub.Example.Avalonia.Android")
                ?? Type.GetType("Iaphub.Example.Avalonia.Nuget.Android.Services.AndroidDialogService, Iaphub.Example.Avalonia.Nuget.Android");

            if (androidDialogServiceType != null)
            {
                services.AddSingleton(typeof(IDialogService), androidDialogServiceType);
            }
            else
            {
                services.AddSingleton<IDialogService, global::Iaphub.Example.Avalonia.Services.DialogService>();
            }
        }
        else
        {
            services.AddSingleton<IDialogService, global::Iaphub.Example.Avalonia.Services.DialogService>();
        }

        _serviceProvider = services.BuildServiceProvider();
    }
}
