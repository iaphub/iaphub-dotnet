using Foundation;
using Avalonia;
using Avalonia.iOS;
using Iaphub.Example.Avalonia;

namespace Iaphub.Example.Avalonia.Nuget.iOS;

[Register("AppDelegate")]
public partial class AppDelegate : AvaloniaAppDelegate<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder);
    }
}
