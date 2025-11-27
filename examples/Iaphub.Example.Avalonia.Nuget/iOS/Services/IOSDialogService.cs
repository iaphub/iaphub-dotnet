using System.Threading.Tasks;
using UIKit;
using Iaphub.Example.Shared.Services;

namespace Iaphub.Example.Avalonia.Nuget.iOS.Services;

public class IOSDialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message)
    {
        var tcs = new TaskCompletionSource<bool>();

        var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, _ => tcs.SetResult(true)));

        var viewController = GetTopViewController();
        viewController?.PresentViewController(alert, true, null);

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

        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, _ =>
        {
            tcs.SetResult(alert.TextFields[0].Text);
        }));

        alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, _ =>
        {
            tcs.SetResult(null);
        }));

        var viewController = GetTopViewController();
        viewController?.PresentViewController(alert, true, null);

        return tcs.Task;
    }

    private UIViewController? GetTopViewController()
    {
        var window = UIApplication.SharedApplication.KeyWindow;
        var rootViewController = window?.RootViewController;

        while (rootViewController?.PresentedViewController != null)
        {
            rootViewController = rootViewController.PresentedViewController;
        }

        return rootViewController;
    }
}
