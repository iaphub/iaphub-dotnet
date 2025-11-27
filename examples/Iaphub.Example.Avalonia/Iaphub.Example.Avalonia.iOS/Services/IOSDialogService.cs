using System.Linq;
using System.Threading.Tasks;
using Iaphub.Example.Shared.Services;
using UIKit;

namespace Iaphub.Example.Avalonia.iOS.Services;

public class IOSDialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message)
    {
        var tcs = new TaskCompletionSource<bool>();

        var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, _ =>
        {
            tcs.TrySetResult(true);
        }));

        PresentAlert(alert);
        return tcs.Task;
    }

    public Task<string?> ShowPromptAsync(string title, string message, string? placeholder = null, string? initialValue = null)
    {
        var tcs = new TaskCompletionSource<string?>();

        var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

        alert.AddTextField(textField =>
        {
            textField.Placeholder = placeholder;
            textField.Text = initialValue;
        });

        alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, _ =>
        {
            tcs.TrySetResult(null);
        }));

        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, _ =>
        {
            var text = alert.TextFields?.FirstOrDefault()?.Text;
            tcs.TrySetResult(text);
        }));

        PresentAlert(alert);
        return tcs.Task;
    }

    private void PresentAlert(UIAlertController alert)
    {
        // Use the modern iOS 13+ approach to get the key window
        var window = UIApplication.SharedApplication
            .ConnectedScenes
            .OfType<UIWindowScene>()
            .SelectMany(scene => scene.Windows)
            .FirstOrDefault(window => window.IsKeyWindow);

        var viewController = window?.RootViewController;

        if (viewController != null)
        {
            viewController.PresentViewController(alert, true, null);
        }
    }
}
