using Iaphub.Example.Shared.Services;

namespace Iaphub.Example.Maui.Services;

public class MauiDialogService : IDialogService
{
    public async Task ShowAlertAsync(string title, string message)
    {
        var page = GetCurrentPage();
        if (page != null)
        {
            await page.DisplayAlert(title, message, "OK");
        }
    }

    public async Task<string?> ShowPromptAsync(string title, string message, string? placeholder = null, string? initialValue = null)
    {
        var page = GetCurrentPage();
        if (page != null)
        {
            return await page.DisplayPromptAsync(
                title,
                message,
                placeholder: placeholder,
                initialValue: initialValue
            );
        }
        return null;
    }

    private static Page? GetCurrentPage()
    {
        return Application.Current?.Windows.FirstOrDefault()?.Page;
    }
}
