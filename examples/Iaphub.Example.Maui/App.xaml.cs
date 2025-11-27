using Iaphub.Example.Shared.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Iaphub.Example.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var mainPage = Handler?.MauiContext?.Services.GetRequiredService<MainPage>();
		var mainViewModel = Handler?.MauiContext?.Services.GetRequiredService<MainViewModel>();

		// Initialize ViewModel
		mainViewModel?.Initialize();

		return new Window(mainPage!);
	}
}
