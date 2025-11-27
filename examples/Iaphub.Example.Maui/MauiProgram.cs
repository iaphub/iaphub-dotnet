using Microsoft.Extensions.Logging;
using Iaphub.Example.Shared.ViewModels;
using Iaphub.Example.Shared.Services;
using Iaphub.Example.Maui.Services;

namespace Iaphub.Example.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register services
		builder.Services.AddSingleton<IStoreStateService, StoreStateService>();
		builder.Services.AddSingleton<IDialogService, MauiDialogService>();

		// Register ViewModels
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<StoreViewModel>();

		// Register Pages
		builder.Services.AddSingleton<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
