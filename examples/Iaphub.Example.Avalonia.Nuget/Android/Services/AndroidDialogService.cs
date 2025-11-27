using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Iaphub.Example.Shared.Services;

namespace Iaphub.Example.Avalonia.Nuget.Android.Services;

public class AndroidDialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message)
    {
        var tcs = new TaskCompletionSource<bool>();

        var activity = Application.Context as Activity;
        if (activity == null)
        {
            tcs.SetResult(false);
            return tcs.Task;
        }

        activity.RunOnUiThread(() =>
        {
            var builder = new AlertDialog.Builder(activity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton("OK", (sender, args) =>
            {
                tcs.SetResult(true);
            });
            builder.SetCancelable(false);

            var dialog = builder.Create();
            dialog.Show();
        });

        return tcs.Task;
    }

    public Task<string?> ShowPromptAsync(string title, string message, string? placeholder = null, string? initialValue = null)
    {
        var tcs = new TaskCompletionSource<string?>();

        var activity = Application.Context as Activity;
        if (activity == null)
        {
            tcs.SetResult(null);
            return tcs.Task;
        }

        activity.RunOnUiThread(() =>
        {
            var editText = new EditText(activity);
            if (placeholder != null)
                editText.Hint = placeholder;
            if (initialValue != null)
                editText.Text = initialValue;

            var builder = new AlertDialog.Builder(activity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetView(editText);
            builder.SetPositiveButton("OK", (sender, args) =>
            {
                tcs.SetResult(editText.Text);
            });
            builder.SetNegativeButton("Cancel", (sender, args) =>
            {
                tcs.SetResult(null);
            });
            builder.SetCancelable(false);

            var dialog = builder.Create();
            dialog.Show();
        });

        return tcs.Task;
    }
}
