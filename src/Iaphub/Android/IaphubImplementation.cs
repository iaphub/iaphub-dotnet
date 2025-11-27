using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Android.App;
using Com.Iaphub;
using Iaphub;

namespace Iaphub.Android
{
    public class IaphubImplementation : IIaphub
    {
        // Static property to hold the current Activity for Buy operations
        // This should be set by the application (e.g., in MainActivity.OnCreate)
        public static Activity? CurrentActivity { get; set; }
        private static bool _activityCallbacksRegistered;
        public Task StartAsync(string appId, string apiKey, string? userId = null, bool allowAnonymousPurchase = true, bool enableStorekitV2 = false, string lang = "en")
        {
            var context = Application.Context;
            var version = typeof(IIaphub).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";

            // Set up listeners before starting
            Com.Iaphub.Iaphub.Instance.SetOnDeferredPurchaseListener(new DeferredPurchaseListener(this));
            Com.Iaphub.Iaphub.Instance.SetOnErrorListener(new ErrorListener(this));
            Com.Iaphub.Iaphub.Instance.SetOnReceiptListener(new ReceiptListener(this));
            Com.Iaphub.Iaphub.Instance.SetOnUserUpdateListener(new UserUpdateListener(this));

            TryRegisterActivityCallbacks(context);

            Com.Iaphub.Iaphub.Instance.Start(context, appId, apiKey, userId, allowAnonymousPurchase, true, "production", lang, "dotnet", version);
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            Com.Iaphub.Iaphub.Instance.Stop();
            return Task.CompletedTask;
        }

        public Task LoginAsync(string userId)
        {
            var tcs = new TaskCompletionSource<bool>();
            Com.Iaphub.Iaphub.Instance.Login(userId, new LoginCallback(tcs));
            return tcs.Task;
        }

        public Task LogoutAsync()
        {
            Com.Iaphub.Iaphub.Instance.Logout();
            return Task.CompletedTask;
        }

        public Task SetLangAsync(string lang)
        {
            Com.Iaphub.Iaphub.Instance.SetLang(lang);
            return Task.CompletedTask;
        }

        public Task SetDeviceParamsAsync(Dictionary<string, string> @params)
        {
            Com.Iaphub.Iaphub.Instance.SetDeviceParams(@params);
            return Task.CompletedTask;
        }

        public Task SetUserTagsAsync(Dictionary<string, string> tags)
        {
            var tcs = new TaskCompletionSource<bool>();
            Com.Iaphub.Iaphub.Instance.SetUserTags(tags, new SimpleCallback(tcs));
            return tcs.Task;
        }

        public Task<IaphubProduct[]> GetProductsForSaleAsync()
        {
            var tcs = new TaskCompletionSource<IaphubProduct[]>();
            Com.Iaphub.Iaphub.Instance.GetProductsForSale(new GetProductsCallback(tcs));
            return tcs.Task;
        }

        public Task<IaphubActiveProduct[]> GetActiveProductsAsync(string[]? includeSubscriptionStates = null)
        {
            var tcs = new TaskCompletionSource<IaphubActiveProduct[]>();
            var states = includeSubscriptionStates != null ? new List<string>(includeSubscriptionStates) : new List<string>();
            Com.Iaphub.Iaphub.Instance.GetActiveProducts(states, new GetActiveProductsCallback(tcs));
            return tcs.Task;
        }

        public Task<IaphubProducts> GetProductsAsync(string[]? includeSubscriptionStates = null)
        {
            var tcs = new TaskCompletionSource<IaphubProducts>();
            var states = includeSubscriptionStates != null ? new List<string>(includeSubscriptionStates) : new List<string>();
            Com.Iaphub.Iaphub.Instance.GetProducts(states, new GetAllProductsCallback(tcs));
            return tcs.Task;
        }

        public Task<IaphubReceiptTransaction> BuyAsync(string sku, bool crossPlatformConflict = false, string? prorationMode = null)
        {
            if (CurrentActivity == null)
            {
                throw new InvalidOperationException(
                    "CurrentActivity is null. Please set IaphubImplementation.CurrentActivity " +
                    "in your MainActivity.OnCreate or OnResume method.");
            }

            var tcs = new TaskCompletionSource<IaphubReceiptTransaction>();
            Com.Iaphub.Iaphub.Instance.Buy(
                CurrentActivity,
                sku,
                prorationMode,
                crossPlatformConflict,
                new BuyCallback(tcs)
            );
            return tcs.Task;
        }

        public Task<IaphubRestoreResponse> RestoreAsync()
        {
            var tcs = new TaskCompletionSource<IaphubRestoreResponse>();
            Com.Iaphub.Iaphub.Instance.Restore(new RestoreCallback(tcs));
            return tcs.Task;
        }

        public Task ShowManageSubscriptionsAsync(string? sku = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            Com.Iaphub.Iaphub.Instance.ShowManageSubscriptions(sku, new SimpleCallback(tcs));
            return tcs.Task;
        }

        public Task PresentCodeRedemptionSheetAsync()
        {
            // Android SDK doesn't have PresentCodeRedemptionSheet - this is iOS only
            // Return completed task to maintain interface compatibility
            return Task.CompletedTask;
        }

        public Task<string> GetUserIdAsync()
        {
            return Task.FromResult(Com.Iaphub.Iaphub.Instance.UserId ?? string.Empty);
        }

        public Task<IaphubBillingStatus> GetBillingStatusAsync()
        {
            var nativeStatus = Com.Iaphub.Iaphub.Instance.BillingStatus;
            return Task.FromResult(nativeStatus?.ToModel() ?? new IaphubBillingStatus());
        }

        private static void TryRegisterActivityCallbacks(global::Android.Content.Context context)
        {
            if (_activityCallbacksRegistered)
            {
                return;
            }

            if (context is Application application)
            {
                application.RegisterActivityLifecycleCallbacks(new ActivityLifecycleCallbacks());
                _activityCallbacksRegistered = true;
            }
        }

#pragma warning disable CS0067 // Event is never used - iOS only, required by interface
        public event EventHandler<string>? OnBuyRequest;
#pragma warning restore CS0067
        public event EventHandler<IaphubReceiptTransaction>? OnDeferredPurchase;
        public event EventHandler? OnUserUpdate;
        public event EventHandler<IaphubError>? OnError;
        public event EventHandler<(IaphubError? err, IaphubReceipt? receipt)>? OnProcessReceipt;

        // Helper method to convert Android.Runtime.JavaList to Java.Util.IList
        private static Java.Util.IList? ConvertToJavaList(Java.Lang.Object? obj)
        {
            if (obj == null) return null;

            // Try Android.Runtime.JavaList first (generic wrapper)
            if (obj is System.Collections.IList dotnetList)
            {
                var javaList = new Java.Util.ArrayList();
                foreach (var item in dotnetList)
                {
                    javaList.Add(item as Java.Lang.Object);
                }
                return javaList;
            }
            // Try Java.Util.IList
            else if (obj is Java.Util.IList list)
            {
                return list;
            }
            // Try Java.Util.ICollection
            else if (obj is Java.Util.ICollection collection)
            {
                var javaList = new Java.Util.ArrayList();
                var iterator = collection.Iterator();
                while (iterator!.HasNext)
                {
                    javaList.Add(iterator.Next());
                }
                return javaList;
            }

            return null;
        }

        // Callbacks implementation
        class LoginCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1
        {
            TaskCompletionSource<bool> _tcs;
            public LoginCallback(TaskCompletionSource<bool> tcs) { _tcs = tcs; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0)
            {
                var err = p0 as Com.Iaphub.IaphubError;
                if (err != null) _tcs.SetException(new IaphubException(err.ToModel()));
                else _tcs.SetResult(true);
                return null;
            }
        }

        // Listener implementations for Android SDK
        class DeferredPurchaseListener : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1
        {
            IaphubImplementation _parent;
            public DeferredPurchaseListener(IaphubImplementation parent) { _parent = parent; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0)
            {
                var transaction = p0 as Com.Iaphub.ReceiptTransaction;
                if (transaction != null) _parent.OnDeferredPurchase?.Invoke(_parent, transaction.ToModel());
                return null;
            }
        }

        class ErrorListener : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1
        {
            IaphubImplementation _parent;
            public ErrorListener(IaphubImplementation parent) { _parent = parent; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0)
            {
                var err = p0 as Com.Iaphub.IaphubError;
                if (err != null) _parent.OnError?.Invoke(_parent, err.ToModel());
                return null;
            }
        }

        class ReceiptListener : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
        {
            IaphubImplementation _parent;
            public ReceiptListener(IaphubImplementation parent) { _parent = parent; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
            {
                var err = p0 as Com.Iaphub.IaphubError;
                var receipt = p1 as Com.Iaphub.Receipt;
                _parent.OnProcessReceipt?.Invoke(_parent, (err?.ToModel(), receipt?.ToModel()));
                return null;
            }
        }

        class UserUpdateListener : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction0
        {
            IaphubImplementation _parent;
            public UserUpdateListener(IaphubImplementation parent) { _parent = parent; }
            public Java.Lang.Object? Invoke()
            {
                _parent.OnUserUpdate?.Invoke(_parent, EventArgs.Empty);
                return null;
            }
        }

        class ActivityLifecycleCallbacks : Java.Lang.Object, Application.IActivityLifecycleCallbacks
        {
            public void OnActivityCreated(Activity activity, global::Android.OS.Bundle? savedInstanceState)
            {
                IaphubImplementation.CurrentActivity = activity;
            }

            public void OnActivityDestroyed(Activity activity)
            {
                if (ReferenceEquals(IaphubImplementation.CurrentActivity, activity))
                {
                    IaphubImplementation.CurrentActivity = null;
                }
            }

            public void OnActivityPaused(Activity activity)
            {
            }

            public void OnActivityResumed(Activity activity)
            {
                IaphubImplementation.CurrentActivity = activity;
            }

            public void OnActivitySaveInstanceState(Activity activity, global::Android.OS.Bundle outState)
            {
            }

            public void OnActivityStarted(Activity activity)
            {
                IaphubImplementation.CurrentActivity = activity;
            }

            public void OnActivityStopped(Activity activity)
            {
                if (ReferenceEquals(IaphubImplementation.CurrentActivity, activity))
                {
                    IaphubImplementation.CurrentActivity = null;
                }
            }
        }

        class SimpleCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1
        {
            TaskCompletionSource<bool> _tcs;
            public SimpleCallback(TaskCompletionSource<bool> tcs) { _tcs = tcs; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0)
            {
                var err = p0 as Com.Iaphub.IaphubError;
                if (err != null) _tcs.SetException(new IaphubException(err.ToModel()));
                else _tcs.SetResult(true);
                return null;
            }
        }

        class GetProductsCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
        {
            TaskCompletionSource<IaphubProduct[]> _tcs;
            public GetProductsCallback(TaskCompletionSource<IaphubProduct[]> tcs) { _tcs = tcs; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
            {
                try
                {
                    var err = p0 as Com.Iaphub.IaphubError;
                    if (err != null)
                    {
                        _tcs.SetException(new IaphubException(err.ToModel()));
                    }
                    else
                    {
                        var javaList = ConvertToJavaList(p1);
                        if (javaList != null)
                        {
                            var products = new List<IaphubProduct>();
                            for (int i = 0; i < javaList.Size(); i++)
                            {
                                var nativeProduct = javaList.Get(i) as Com.Iaphub.Product;
                                if (nativeProduct != null)
                                {
                                    products.Add(nativeProduct.ToModel());
                                }
                            }
                            _tcs.SetResult(products.ToArray());
                        }
                        else
                        {
                            _tcs.SetResult(Array.Empty<IaphubProduct>());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _tcs.SetException(ex);
                }
                return null;
            }
        }

        class GetActiveProductsCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
        {
            TaskCompletionSource<IaphubActiveProduct[]> _tcs;
            public GetActiveProductsCallback(TaskCompletionSource<IaphubActiveProduct[]> tcs) { _tcs = tcs; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
            {
                try
                {
                    var err = p0 as Com.Iaphub.IaphubError;
                    if (err != null)
                    {
                        _tcs.SetException(new IaphubException(err.ToModel()));
                    }
                    else
                    {
                        var javaList = ConvertToJavaList(p1);
                        if (javaList != null)
                        {
                            var products = new List<IaphubActiveProduct>();
                            for (int i = 0; i < javaList.Size(); i++)
                            {
                                var nativeProduct = javaList.Get(i) as Com.Iaphub.ActiveProduct;
                                if (nativeProduct != null)
                                {
                                    products.Add(nativeProduct.ToModel());
                                }
                            }
                            _tcs.SetResult(products.ToArray());
                        }
                        else
                        {
                            _tcs.SetResult(Array.Empty<IaphubActiveProduct>());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _tcs.SetException(ex);
                }
                return null;
            }
        }

        class GetAllProductsCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
        {
            TaskCompletionSource<IaphubProducts> _tcs;
            public GetAllProductsCallback(TaskCompletionSource<IaphubProducts> tcs) { _tcs = tcs; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
            {
                try
                {
                    var err = p0 as Com.Iaphub.IaphubError;
                    if (err != null)
                    {
                        _tcs.SetException(new IaphubException(err.ToModel()));
                    }
                    else
                    {
                        var nativeProducts = p1 as Com.Iaphub.Products;
                        _tcs.SetResult(nativeProducts?.ToModel() ?? new IaphubProducts());
                    }
                }
                catch (Exception ex)
                {
                    _tcs.SetException(ex);
                }
                return null;
            }
        }

        class RestoreCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
        {
            TaskCompletionSource<IaphubRestoreResponse> _tcs;
            public RestoreCallback(TaskCompletionSource<IaphubRestoreResponse> tcs) { _tcs = tcs; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
            {
                try
                {
                    var err = p0 as Com.Iaphub.IaphubError;
                    if (err != null)
                    {
                        _tcs.SetException(new IaphubException(err.ToModel()));
                    }
                    else
                    {
                        var nativeResponse = p1 as Com.Iaphub.RestoreResponse;
                        _tcs.SetResult(nativeResponse?.ToModel() ?? new IaphubRestoreResponse());
                    }
                }
                catch (Exception ex)
                {
                    _tcs.SetException(ex);
                }
                return null;
            }
        }

        class BuyCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
        {
            TaskCompletionSource<IaphubReceiptTransaction> _tcs;
            public BuyCallback(TaskCompletionSource<IaphubReceiptTransaction> tcs) { _tcs = tcs; }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
            {
                var err = p0 as Com.Iaphub.IaphubError;
                if (err != null)
                {
                    _tcs.SetException(new IaphubException(err.ToModel()));
                }
                else
                {
                    var transaction = p1 as Com.Iaphub.ReceiptTransaction;
                    _tcs.SetResult(transaction?.ToModel() ?? new IaphubReceiptTransaction());
                }
                return null;
            }
        }
    }

    internal static class IaphubPlatformRegistration
    {
        [ModuleInitializer]
        internal static void Register() => global::Iaphub.Sdk.IaphubSdk.Init(new IaphubImplementation());
    }
}
