using System;
using System.Threading.Tasks;
using Android.App;
using Android.Text;
using Android.Widget;
using Iaphub.Example.Shared.Services;

namespace Iaphub.Example.Avalonia.Android.Services;

public class AndroidDialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message)
    {
        var tcs = new TaskCompletionSource<bool>();

        var activity = MainActivity.Instance;
        if (activity == null)
        {
            tcs.SetResult(false);
            return tcs.Task;
        }

        activity.RunOnUiThread(() =>
        {
            try
            {
                var builder = new AlertDialog.Builder(activity);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetPositiveButton("OK", (sender, args) =>
                {
                    tcs.TrySetResult(true);
                });
                builder.SetCancelable(false);
                var dialog = builder.Create();
                dialog?.Show();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing alert: {ex.Message}");
                tcs.TrySetResult(false);
            }
        });

        return tcs.Task;
    }

    public Task<string?> ShowPromptAsync(string title, string message, string? placeholder = null, string? initialValue = null)
    {
        var tcs = new TaskCompletionSource<string?>();

        var activity = MainActivity.Instance;
        if (activity == null)
        {
            tcs.SetResult(null);
            return tcs.Task;
        }

        activity.RunOnUiThread(() =>
        {
            try
            {
                var input = new EditText(activity);
                input.Hint = placeholder;
                input.Text = initialValue;
                input.InputType = InputTypes.ClassText;

                var builder = new AlertDialog.Builder(activity);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetView(input);
                builder.SetPositiveButton("OK", (sender, args) =>
                {
                    tcs.TrySetResult(input.Text);
                });
                builder.SetNegativeButton("Cancel", (sender, args) =>
                {
                    tcs.TrySetResult(null);
                });
                builder.SetCancelable(false);
                var dialog = builder.Create();
                dialog?.Show();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing prompt: {ex.Message}");
                tcs.TrySetResult(null);
            }
        });

        return tcs.Task;
    }
}
