using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Iaphub.Example.Avalonia;

namespace Iaphub.Example.Avalonia.Nuget.Android;

[Activity(
    Theme = "@style/MyTheme.NoActionBar",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
}
