using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iaphub
{
    [Obsolete("Use Iaphub.Sdk.IaphubSdk static methods for the public API.")]
    public static class Iaphub
    {
        public static IIaphub Current
        {
            get => Sdk.IaphubSdk.Current;
            set => Sdk.IaphubSdk.Init(value);
        }

        public static void Init(IIaphub implementation)
        {
            Sdk.IaphubSdk.Init(implementation);
        }
    }
}

namespace Iaphub.Sdk
{
    public static class IaphubSdk
    {
        private static global::Iaphub.IIaphub? _current;

        internal static global::Iaphub.IIaphub Current
        {
            get
            {
                if (_current == null)
                {
                    throw new NotImplementedException("Iaphub implementation not initialized. Call IaphubSdk.Init(...) with the platform implementation.");
                }

                return _current;
            }
            set => _current = value;
        }

        internal static void Init(global::Iaphub.IIaphub implementation)
        {
            // Only register once; ignore subsequent registrations to avoid overriding a user-provided implementation.
            if (_current == null)
            {
                Current = implementation;
            }
        }

        public static Task StartAsync(
            string appId,
            string apiKey,
            string? userId = null,
            bool allowAnonymousPurchase = true,
            bool enableStorekitV2 = false,
            string lang = "en")
        {
            return Current.StartAsync(appId, apiKey, userId, allowAnonymousPurchase, enableStorekitV2, lang);
        }

        public static Task StopAsync() => Current.StopAsync();

        public static Task LoginAsync(string userId) => Current.LoginAsync(userId);

        public static Task LogoutAsync() => Current.LogoutAsync();

        public static Task SetLangAsync(string lang) => Current.SetLangAsync(lang);

        public static Task SetDeviceParamsAsync(Dictionary<string, string> @params) => Current.SetDeviceParamsAsync(@params);

        public static Task SetUserTagsAsync(Dictionary<string, string> tags) => Current.SetUserTagsAsync(tags);

        public static Task<global::Iaphub.IaphubProduct[]> GetProductsForSaleAsync() => Current.GetProductsForSaleAsync();

        public static Task<global::Iaphub.IaphubActiveProduct[]> GetActiveProductsAsync(string[]? includeSubscriptionStates = null) =>
            Current.GetActiveProductsAsync(includeSubscriptionStates);

        public static Task<global::Iaphub.IaphubProducts> GetProductsAsync(string[]? includeSubscriptionStates = null) =>
            Current.GetProductsAsync(includeSubscriptionStates);

        public static Task<global::Iaphub.IaphubReceiptTransaction> BuyAsync(string sku, bool crossPlatformConflict = false) =>
            Current.BuyAsync(sku, crossPlatformConflict);

        public static Task<global::Iaphub.IaphubRestoreResponse> RestoreAsync() => Current.RestoreAsync();

        public static Task ShowManageSubscriptionsAsync() => Current.ShowManageSubscriptionsAsync();

        public static Task PresentCodeRedemptionSheetAsync() => Current.PresentCodeRedemptionSheetAsync();

        public static Task<string> GetUserIdAsync() => Current.GetUserIdAsync();

        public static Task<global::Iaphub.IaphubBillingStatus> GetBillingStatusAsync() => Current.GetBillingStatusAsync();

        public static event EventHandler<string> OnBuyRequest
        {
            add => Current.OnBuyRequest += value;
            remove => Current.OnBuyRequest -= value;
        }

        public static event EventHandler<global::Iaphub.IaphubReceiptTransaction> OnDeferredPurchase
        {
            add => Current.OnDeferredPurchase += value;
            remove => Current.OnDeferredPurchase -= value;
        }

        public static event EventHandler OnUserUpdate
        {
            add => Current.OnUserUpdate += value;
            remove => Current.OnUserUpdate -= value;
        }

        public static event EventHandler<global::Iaphub.IaphubError> OnError
        {
            add => Current.OnError += value;
            remove => Current.OnError -= value;
        }

        public static event EventHandler<(global::Iaphub.IaphubError? err, global::Iaphub.IaphubReceipt? receipt)> OnProcessReceipt
        {
            add => Current.OnProcessReceipt += value;
            remove => Current.OnProcessReceipt -= value;
        }
    }
}
