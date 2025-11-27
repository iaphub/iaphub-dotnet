using System;
using System.Linq;
using Foundation;
using Iaphub;
using IaphubBinding.iOS;

namespace Iaphub.iOS
{
    public static class Extensions
    {
        public static IaphubProduct ToModel(this IHProduct native)
        {
            if (native == null) return null!;
            
            return new IaphubProduct
            {
                Id = native.Id,
                Type = native.Type,
                Group = native.Group,
                GroupName = native.GroupName,
                Alias = native.Alias,
                Metadata = native.Metadata?.ToDictionary() ?? new System.Collections.Generic.Dictionary<string, string>(),
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

        public static IaphubSubscriptionIntroPhase ToModel(this IHSubscriptionIntroPhase native)
        {
            if (native == null) return null!;

            return new IaphubSubscriptionIntroPhase
            {
                Type = native.Type,
                Price = (decimal)native.Price,
                Currency = native.Currency,
                LocalizedPrice = native.LocalizedPrice,
                CycleDuration = native.CycleDuration,
                CycleCount = (int)native.CycleCount,
                Payment = native.Payment
            };
        }

        public static IaphubActiveProduct ToModel(this IHActiveProduct native)
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

        public static IaphubProducts ToModel(this IHProducts native)
        {
            if (native == null) return null!;
            
            return new IaphubProducts
            {
                ProductsForSale = native.ProductsForSale?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubProduct>(),
                ActiveProducts = native.ActiveProducts?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubActiveProduct>()
            };
        }

        public static IaphubReceiptTransaction ToModel(this IHReceiptTransaction native)
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

        public static IaphubRestoreResponse ToModel(this IHRestoreResponse native)
        {
            if (native == null) return null!;
            
            return new IaphubRestoreResponse
            {
                NewPurchases = native.NewPurchases?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubReceiptTransaction>(),
                TransferredActiveProducts = native.TransferredActiveProducts?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<IaphubActiveProduct>()
            };
        }

        public static IaphubBillingStatus ToModel(this IHBillingStatus native)
        {
            if (native == null) return null!;
            
            return new IaphubBillingStatus
            {
                Error = native.Error?.ToModel(),
                FilteredProductIds = native.FilteredProductIds ?? Array.Empty<string>()
            };
        }

        public static IaphubError ToModel(this IHError native)
        {
            if (native == null) return null!;
            
            return new IaphubError
            {
                Code = native.Code,
                Message = native.Message,
                Subcode = native.Subcode,
                Params = native.Params?.ToDictionaryObject() ?? new System.Collections.Generic.Dictionary<string, object>()
            };
        }

        public static IaphubReceipt ToModel(this IHReceipt native)
        {
            if (native == null) return null!;
            
            return new IaphubReceipt
            {
                Token = native.Token,
                Sku = native.Sku,
                Context = native.Context,
                PaymentProcessor = native.PaymentProcessor,
                IsFinished = native.IsFinished,
                ProcessDate = native.ProcessDate?.ToDateTime()
            };
        }

        public static DateTime? ToDateTime(this NSDate date)
        {
            if (date == null) return null;
            
            var reference = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static System.Collections.Generic.Dictionary<string, string> ToDictionary(this NSDictionary<NSString, NSString> source)
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>();
            if (source == null) return dict;

            foreach (var (key, value) in source)
            {
                dict.Add(key.ToString(), value.ToString());
            }
            return dict;
        }

        public static System.Collections.Generic.Dictionary<string, object> ToDictionaryObject(this NSDictionary<NSString, NSObject> source)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (source == null) return dict;

            foreach (var (key, value) in source)
            {
                dict.Add(key.ToString(), value);
            }
            return dict;
        }
    }
}
