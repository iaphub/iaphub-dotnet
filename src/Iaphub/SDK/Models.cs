using System;
using System.Collections.Generic;

namespace Iaphub
{
    public class IaphubProduct : IaphubProductDetails
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = "";
        public string? Group { get; set; }
        public string? GroupName { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
        public string? Alias { get; set; }
    }

    public class IaphubProductDetails
    {
        public string Sku { get; set; } = "";
        public string? LocalizedTitle { get; set; }
        public string? LocalizedDescription { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public string? LocalizedPrice { get; set; }
        public string? SubscriptionDuration { get; set; }
        public IaphubSubscriptionIntroPhase[]? SubscriptionIntroPhases { get; set; }
    }

    public class IaphubActiveProduct : IaphubProduct
    {
        public string? Purchase { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Platform { get; set; }
        public bool IsSandbox { get; set; }
        public bool IsPromo { get; set; }
        public string? PromoCode { get; set; }
        public string? OriginalPurchase { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsSubscriptionRenewable { get; set; }
        public bool IsFamilyShare { get; set; }
        public string? SubscriptionRenewalProduct { get; set; }
        public string? SubscriptionRenewalProductSku { get; set; }
        public string? SubscriptionState { get; set; }
        public string? SubscriptionPeriodType { get; set; }
    }

    public class IaphubReceiptTransaction : IaphubActiveProduct
    {
        public string? WebhookStatus { get; set; }
        public string? User { get; set; }
    }

    public class IaphubProducts
    {
        public IaphubProduct[] ProductsForSale { get; set; } = Array.Empty<IaphubProduct>();
        public IaphubActiveProduct[] ActiveProducts { get; set; } = Array.Empty<IaphubActiveProduct>();
    }

    public class IaphubBillingStatus
    {
        public IaphubError? Error { get; set; }
        public string[] FilteredProductIds { get; set; } = Array.Empty<string>();
    }

    public class IaphubError
    {
        public string Message { get; set; } = "";
        public string Code { get; set; } = "";
        public string? Subcode { get; set; }
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }

    public class IaphubReceipt
    {
        public string Token { get; set; } = "";
        public string Sku { get; set; } = "";
        public string Context { get; set; } = "";
        public string PaymentProcessor { get; set; } = "";
        public bool IsFinished { get; set; }
        public DateTime? ProcessDate { get; set; }
    }

    public class IaphubRestoreResponse
    {
        public IaphubReceiptTransaction[] NewPurchases { get; set; } = Array.Empty<IaphubReceiptTransaction>();
        public IaphubActiveProduct[] TransferredActiveProducts { get; set; } = Array.Empty<IaphubActiveProduct>();
    }

    public class IaphubSubscriptionIntroPhase
    {
        public string Type { get; set; } = "";
        public decimal Price { get; set; }
        public string Currency { get; set; } = "";
        public string LocalizedPrice { get; set; } = "";
        public string CycleDuration { get; set; } = "";
        public int CycleCount { get; set; }
        public string Payment { get; set; } = "";
    }
}
