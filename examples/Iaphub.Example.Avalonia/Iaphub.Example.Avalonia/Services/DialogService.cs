using System;
using System.Threading.Tasks;
using Iaphub.Example.Shared.Services;

namespace Iaphub.Example.Avalonia.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message)
    {
        Console.WriteLine($"{title}: {message}");
        return Task.CompletedTask;
    }

    public Task<string?> ShowPromptAsync(string title, string message, string? placeholder = null, string? initialValue = null)
    {
        Console.WriteLine($"{title}: {message}");
        // For desktop Avalonia, the modal in the UI will be used instead
        return Task.FromResult(initialValue);
    }
}
