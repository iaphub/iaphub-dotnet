using System;
using System.Linq;
using Com.Iaphub;
using Iaphub;

namespace Iaphub.Android
{
    public static class Extensions
    {
        public static IaphubProduct ToModel(this Com.Iaphub.Product native)
        {
            if (native == null) return null!;
            return new IaphubProduct
            {
                Id = native.Id,
                Type = native.Type,
                Group = native.Group,
                GroupName = native.GroupName,
                Metadata = native.Metadata?.ToDictionary() ?? new System.Collections.Generic.Dictionary<string, string>(),
                Alias = native.Alias,
                Sku = native.Sku,
                LocalizedTitle = native.LocalizedTitle,
                LocalizedDescription = native.LocalizedDescription,
                Price = native.Price != null ? (decimal)(double)native.Price : null,
                Currency = native.Currency,
                LocalizedPrice = native.LocalizedPrice,
                SubscriptionDuration = native.SubscriptionDuration,
                SubscriptionIntroPhases = native.SubscriptionIntroPhases?.Select(x => x.ToModel()).ToArray()
            };
        }

        public static IaphubSubscriptionIntroPhase ToModel(this Com.Iaphub.SubscriptionIntroPhase native)
        {
            if (native == null) return null!;
            return new IaphubSubscriptionIntroPhase
            {
                Type = native.Type,
                Price = (decimal)(double)native.Price,
                Currency = native.Currency,
                LocalizedPrice = native.LocalizedPrice,
                CycleDuration = native.CycleDuration,
                CycleCount = (int)native.CycleCount,
                Payment = native.Payment
            };
        }

        public static IaphubActiveProduct ToModel(this Com.Iaphub.ActiveProduct native)
        {
            if (native == null) return null!;
            return new IaphubActiveProduct
            {
                Id = native.Id,
                Type = native.Type,
                Group = native.Group,
                GroupName = native.GroupName,
                Sku = native.Sku,
                LocalizedTitle = native.LocalizedTitle,
                LocalizedDescription = native.LocalizedDescription,
                Price = native.Price != null ? (decimal)(double)native.Price : null,
                Currency = native.Currency,
                LocalizedPrice = native.LocalizedPrice,
                SubscriptionDuration = native.SubscriptionDuration,
                SubscriptionIntroPhases = native.SubscriptionIntroPhases?.Select(x => x.ToModel()).ToArray(),
                Purchase = native.Purchase,
                PurchaseDate = native.PurchaseDate?.ToDateTime(),
                Platform = native.Platform,
                IsSandbox = native.IsSandbox,
                IsPromo = native.IsPromo,
                PromoCode = native.PromoCode,
                OriginalPurchase = native.OriginalPurchase,
                ExpirationDate = native.ExpirationDate?.ToDateTime(),
                IsSubscriptionRenewable = native.IsSubscriptionRenewable,
                IsFamilyShare = native.IsFamilyShare,
                SubscriptionRenewalProduct = native.SubscriptionRenewalProduct,
                SubscriptionRenewalProductSku = native.SubscriptionRenewalProductSku,
                SubscriptionState = native.SubscriptionState,
                SubscriptionPeriodType = native.SubscriptionPeriodType
            };
        }

        public static IaphubReceiptTransaction ToModel(this Com.Iaphub.ReceiptTransaction native)
        {
            if (native == null) return null!;
            return new IaphubReceiptTransaction
            {
                Id = native.Id,
                Type = native.Type,
                Group = native.Group,
                GroupName = native.GroupName,
                Sku = native.Sku,
                LocalizedTitle = native.LocalizedTitle,
                LocalizedDescription = native.LocalizedDescription,
                Price = native.Price != null ? (decimal)(double)native.Price : null,
                Currency = native.Currency,
                LocalizedPrice = native.LocalizedPrice,
                SubscriptionDuration = native.SubscriptionDuration,
                SubscriptionIntroPhases = native.SubscriptionIntroPhases?.Select(x => x.ToModel()).ToArray(),
                Purchase = native.Purchase,
                PurchaseDate = native.PurchaseDate?.ToDateTime(),
                Platform = native.Platform,
                IsSandbox = native.IsSandbox,
                IsPromo = native.IsPromo,
                PromoCode = native.PromoCode,
                OriginalPurchase = native.OriginalPurchase,
                ExpirationDate = native.ExpirationDate?.ToDateTime(),
                IsSubscriptionRenewable = native.IsSubscriptionRenewable,
                IsFamilyShare = native.IsFamilyShare,
                SubscriptionRenewalProduct = native.SubscriptionRenewalProduct,
                SubscriptionRenewalProductSku = native.SubscriptionRenewalProductSku,
                SubscriptionState = native.SubscriptionState,
                SubscriptionPeriodType = native.SubscriptionPeriodType,
                WebhookStatus = native.WebhookStatus,
                User = native.User
            };
        }

        public static IaphubProducts ToModel(this Com.Iaphub.Products native)
        {
            if (native == null) return null!;
            return new IaphubProducts
            {
                ProductsForSale = native.ProductsForSale?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubProduct>(),
                ActiveProducts = native.ActiveProducts?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubActiveProduct>()
            };
        }

        public static IaphubError ToModel(this Com.Iaphub.IaphubError native)
        {
            if (native == null) return null!;
            return new IaphubError
            {
                Message = native.Message,
                Code = native.Code,
                Subcode = native.Subcode,
                Params = native.Params?.ToDictionaryObject() ?? new System.Collections.Generic.Dictionary<string, object>()
            };
        }

        public static IaphubReceipt ToModel(this Com.Iaphub.Receipt native)
        {
            if (native == null) return null!;
            return new IaphubReceipt
            {
                Token = native.Token,
                Sku = native.Sku,
                Context = native.Context,
                ProcessDate = native.ProcessDate?.ToDateTime()
                // Note: PaymentProcessor and IsFinished don't exist in Android SDK
            };
        }

        public static IaphubRestoreResponse ToModel(this Com.Iaphub.RestoreResponse native)
        {
            if (native == null) return null!;
            return new IaphubRestoreResponse
            {
                NewPurchases = native.NewPurchases?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubReceiptTransaction>(),
                TransferredActiveProducts = native.TransferredActiveProducts?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubActiveProduct>()
            };
        }

        public static IaphubBillingStatus ToModel(this Com.Iaphub.BillingStatus native)
        {
            if (native == null) return null!;
            return new IaphubBillingStatus
            {
                Error = native.Error?.ToModel(),
                FilteredProductIds = native.FilteredProductIds?.ToArray() ?? Array.Empty<string>()
            };
        }

        public static DateTime? ToDateTime(this Java.Util.Date date)
        {
            if (date == null) return null;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(date.Time);
        }

        public static System.Collections.Generic.Dictionary<string, string> ToDictionary(this System.Collections.Generic.IDictionary<string, string> source)
        {
            return source?.ToDictionary(k => k.Key, v => v.Value) ?? new System.Collections.Generic.Dictionary<string, string>();
        }

        public static System.Collections.Generic.Dictionary<string, object> ToDictionaryObject(this System.Collections.Generic.IDictionary<string, Java.Lang.Object> source)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (source == null) return dict;

            foreach (var kvp in source)
            {
                dict.Add(kvp.Key, kvp.Value.ToString());
            }
            return dict;
        }
    }
}
