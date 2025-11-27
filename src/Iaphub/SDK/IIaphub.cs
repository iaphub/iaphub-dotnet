using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iaphub
{
    public interface IIaphub
    {
        Task StartAsync(string appId, string apiKey, string? userId = null, bool allowAnonymousPurchase = true, bool enableStorekitV2 = false, string lang = "en");
        Task StopAsync();
        Task LoginAsync(string userId);
        Task LogoutAsync();
        Task SetLangAsync(string lang);
        Task SetDeviceParamsAsync(Dictionary<string, string> @params);
        Task SetUserTagsAsync(Dictionary<string, string> tags);
        Task<IaphubProduct[]> GetProductsForSaleAsync();
        Task<IaphubActiveProduct[]> GetActiveProductsAsync(string[]? includeSubscriptionStates = null);
        Task<IaphubProducts> GetProductsAsync(string[]? includeSubscriptionStates = null);
        Task<IaphubReceiptTransaction> BuyAsync(string sku, bool crossPlatformConflict = false, string? prorationMode = null);
        Task<IaphubRestoreResponse> RestoreAsync();
        Task ShowManageSubscriptionsAsync(string? sku = null);
        Task PresentCodeRedemptionSheetAsync();
        Task<string> GetUserIdAsync();
        Task<IaphubBillingStatus> GetBillingStatusAsync();

        event EventHandler<string> OnBuyRequest;
        event EventHandler<IaphubReceiptTransaction> OnDeferredPurchase;
        event EventHandler OnUserUpdate;
        event EventHandler<IaphubError> OnError;
        event EventHandler<(IaphubError? err, IaphubReceipt? receipt)> OnProcessReceipt;
    }
}
