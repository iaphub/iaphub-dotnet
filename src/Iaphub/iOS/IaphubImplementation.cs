using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Foundation;
using ObjCRuntime;
using Iaphub;
using IaphubBinding.iOS;

namespace Iaphub.iOS
{
    [Register("IaphubImplementation")]
    [Preserve(AllMembers = true)]
    public class IaphubImplementation : NSObject, IIaphub, IIaphubDelegate
    {
        public IaphubImplementation(NativeHandle handle) : base(handle)
        {
        }
        public Task StartAsync(string appId, string apiKey, string? userId = null, bool allowAnonymousPurchase = true, bool enableStorekitV2 = false, string lang = "en")
        {
            var version = typeof(IIaphub).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";
            
            IaphubBinding.iOS.Iaphub.Start(
                appId, 
                apiKey, 
                userId, 
                allowAnonymousPurchase, 
                enableDeferredPurchaseListener: true,
                enableStorekitV2,
                environment: "production", 
                lang,
                sdk: "dotnet", 
                sdkVersion: version
            );
            return Task.CompletedTask;
        }

        public async Task LoginAsync(string userId)
        {
            var tcs = new TaskCompletionSource<object?>();
            IaphubBinding.iOS.Iaphub.Login(userId, (error) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(null);
            });
            await tcs.Task;
        }

        public Task LogoutAsync()
        {
            IaphubBinding.iOS.Iaphub.Logout();
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            IaphubBinding.iOS.Iaphub.Stop();
            return Task.CompletedTask;
        }

        public Task SetLangAsync(string lang)
        {
            IaphubBinding.iOS.Iaphub.SetLang(lang);
            return Task.CompletedTask;
        }

        public async Task<IaphubProduct[]> GetProductsForSaleAsync()
        {
            var tcs = new TaskCompletionSource<IaphubProduct[]>();
            IaphubBinding.iOS.Iaphub.GetProductsForSale((error, products) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(products?.Select(p => p.ToModel()).ToArray() ?? Array.Empty<IaphubProduct>());
            });
            return await tcs.Task;
        }

        public async Task<IaphubActiveProduct[]> GetActiveProductsAsync(string[]? includeSubscriptionStates = null)
        {
            var tcs = new TaskCompletionSource<IaphubActiveProduct[]>();
            IaphubBinding.iOS.Iaphub.GetActiveProducts(includeSubscriptionStates ?? Array.Empty<string>(), (error, products) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(products?.Select(p => p.ToModel()).ToArray() ?? Array.Empty<IaphubActiveProduct>());
            });
            return await tcs.Task;
        }

        public async Task<IaphubProducts> GetProductsAsync(string[]? includeSubscriptionStates = null)
        {
            var tcs = new TaskCompletionSource<IaphubProducts>();
            IaphubBinding.iOS.Iaphub.GetProducts(includeSubscriptionStates ?? Array.Empty<string>(), (error, products) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(products?.ToModel() ?? new IaphubProducts());
            });
            return await tcs.Task;
        }

        public async Task<IaphubReceiptTransaction> BuyAsync(string sku, bool crossPlatformConflict = false, string? prorationMode = null)
        {
            var tcs = new TaskCompletionSource<IaphubReceiptTransaction>();
            // Note: prorationMode is Android-specific, ignored on iOS
            IaphubBinding.iOS.Iaphub.Buy(sku, crossPlatformConflict, (error, transaction) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(transaction?.ToModel() ?? new IaphubReceiptTransaction());
            });
            return await tcs.Task;
        }

        public async Task<IaphubRestoreResponse> RestoreAsync()
        {
            var tcs = new TaskCompletionSource<IaphubRestoreResponse>();
            IaphubBinding.iOS.Iaphub.Restore((error, response) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(response?.ToModel() ?? new IaphubRestoreResponse());
            });
            return await tcs.Task;
        }

        public Task SetDeviceParamsAsync(Dictionary<string, string> deviceParams)
        {
            var dict = new NSMutableDictionary();
            foreach (var kvp in deviceParams)
            {
                dict.SetValueForKey(new NSString(kvp.Value), new NSString(kvp.Key));
            }
            IaphubBinding.iOS.Iaphub.SetDeviceParams(dict);
            return Task.CompletedTask;
        }

        public async Task SetUserTagsAsync(Dictionary<string, string> userTags)
        {
            var dict = new NSMutableDictionary();
            foreach (var kvp in userTags)
            {
                dict.SetValueForKey(new NSString(kvp.Value), new NSString(kvp.Key));
            }
            
            var tcs = new TaskCompletionSource<object?>();
            IaphubBinding.iOS.Iaphub.SetUserTags(dict, (error) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(null);
            });
            await tcs.Task;
        }

        public async Task ShowManageSubscriptionsAsync(string? sku)
        {
            var tcs = new TaskCompletionSource<object?>();
            IaphubBinding.iOS.Iaphub.ShowManageSubscriptions((error) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(null);
            });
            await tcs.Task;
        }

        public async Task PresentCodeRedemptionSheetAsync()
        {
            var tcs = new TaskCompletionSource<object?>();
            IaphubBinding.iOS.Iaphub.PresentCodeRedemptionSheet((error) =>
            {
                if (error != null)
                    tcs.SetException(new IaphubException(error.ToModel()));
                else
                    tcs.SetResult(null);
            });
            await tcs.Task;
        }

        public Task<string> GetUserIdAsync()
        {
            return Task.FromResult(IaphubBinding.iOS.Iaphub.GetUserId() ?? string.Empty);
        }

        public Task<IaphubBillingStatus> GetBillingStatusAsync()
        {
            return Task.FromResult(IaphubBinding.iOS.Iaphub.GetBillingStatus()?.ToModel() ?? new IaphubBillingStatus());
        }

        public event EventHandler<string>? OnBuyRequest;
        public event EventHandler<IaphubReceiptTransaction>? OnDeferredPurchase;
        public event EventHandler<IaphubError>? OnError;
        public event EventHandler<(IaphubError? err, IaphubReceipt? receipt)>? OnProcessReceipt;
        public event EventHandler? OnUserUpdate;

        public IaphubImplementation()
        {
            IaphubBinding.iOS.Iaphub.Delegate = this;
        }

        [Export("didReceiveBuyRequestWithSku:")]
        public void DidReceiveBuyRequest(string sku)
        {
            OnBuyRequest?.Invoke(this, sku);
        }

        [Export("didReceiveDeferredPurchaseWithTransaction:")]
        public void DidReceiveDeferredPurchase(IHReceiptTransaction transaction)
        {
            OnDeferredPurchase?.Invoke(this, transaction.ToModel());
        }

        [Export("didReceiveErrorWithErr:")]
        public void DidReceiveError(IHError error)
        {
            OnError?.Invoke(this, error.ToModel());
        }

        [Export("didProcessReceiptWithErr:receipt:")]
        public void DidProcessReceipt(IHError? err, IHReceipt? receipt)
        {
            OnProcessReceipt?.Invoke(this, (err?.ToModel(), receipt?.ToModel()));
        }

        [Export("didReceiveUserUpdate")]
        public void DidReceiveUserUpdate()
        {
            OnUserUpdate?.Invoke(this, EventArgs.Empty);
        }
    }

    internal static class IaphubPlatformRegistration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA2255")]
        [ModuleInitializer]
        internal static void Register() => global::Iaphub.Sdk.IaphubSdk.Init(new IaphubImplementation());
    }
}
