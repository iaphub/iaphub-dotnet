using System.Threading.Tasks;

namespace Iaphub.Example.Shared.Services;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message);
    Task<string?> ShowPromptAsync(string title, string message, string? placeholder = null, string? initialValue = null);
}
