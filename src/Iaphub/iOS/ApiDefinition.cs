using System;
using Foundation;
using ObjCRuntime;

namespace IaphubBinding.iOS
{
    // @interface IHProductDetails : NSObject
    [BaseType(typeof(NSObject), Name = "IHProductDetails")]
    [DisableDefaultCtor]
    interface IHProductDetails
    {
        // @property (nonatomic, copy) NSString * _Nonnull sku;
        [Export("sku")]
        string Sku { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable localizedTitle;
        [NullAllowed, Export("localizedTitle")]
        string LocalizedTitle { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable localizedDescription;
        [NullAllowed, Export("localizedDescription")]
        string LocalizedDescription { get; set; }

        // @property (nonatomic, strong) NSNumber * _Nullable price;
        [NullAllowed, Export("price", ArgumentSemantic.Strong)]
        NSNumber Price { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable currency;
        [NullAllowed, Export("currency")]
        string Currency { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable localizedPrice;
        [NullAllowed, Export("localizedPrice")]
        string LocalizedPrice { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable subscriptionDuration;
        [NullAllowed, Export("subscriptionDuration")]
        string SubscriptionDuration { get; set; }

        // @property (nonatomic, copy) NSArray<IHSubscriptionIntroPhase *> * _Nullable subscriptionIntroPhases;
        [NullAllowed, Export("subscriptionIntroPhases", ArgumentSemantic.Copy)]
        IHSubscriptionIntroPhase[] SubscriptionIntroPhases { get; set; }
    }

    // @interface IHProduct : IHProductDetails
    [BaseType(typeof(IHProductDetails), Name = "IHProduct")]
    [DisableDefaultCtor]
    interface IHProduct
    {
        // @property (nonatomic, copy) NSString * _Nonnull id;
        [Export("id")]
        string Id { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull type;
        [Export("type")]
        string Type { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable group;
        [NullAllowed, Export("group")]
        string Group { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable groupName;
        [NullAllowed, Export("groupName")]
        string GroupName { get; set; }

        // @property (nonatomic, copy) NSDictionary<NSString *, NSString *> * _Nonnull metadata;
        [Export("metadata", ArgumentSemantic.Copy)]
        NSDictionary<NSString, NSString> Metadata { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable alias;
        [NullAllowed, Export("alias")]
        string Alias { get; set; }

        // @property (nonatomic, strong) IHProductDetails * _Nullable details;
        [NullAllowed, Export("details", ArgumentSemantic.Strong)]
        IHProductDetails Details { get; set; }
    }

    // @interface IHActiveProduct : IHProduct
    [BaseType(typeof(IHProduct), Name = "IHActiveProduct")]
    [DisableDefaultCtor]
    interface IHActiveProduct
    {
        // @property (nonatomic, copy) NSString * _Nullable purchase;
        [NullAllowed, Export("purchase")]
        string Purchase { get; set; }

        // @property (nonatomic, copy) NSDate * _Nullable purchaseDate;
        [NullAllowed, Export("purchaseDate", ArgumentSemantic.Copy)]
        NSDate PurchaseDate { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable platform;
        [NullAllowed, Export("platform")]
        string Platform { get; set; }

        // @property (nonatomic) BOOL isSandbox;
        [Export("isSandbox")]
        bool IsSandbox { get; set; }

        // @property (nonatomic) BOOL isPromo;
        [Export("isPromo")]
        bool IsPromo { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable promoCode;
        [NullAllowed, Export("promoCode")]
        string PromoCode { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable originalPurchase;
        [NullAllowed, Export("originalPurchase")]
        string OriginalPurchase { get; set; }

        // @property (nonatomic, copy) NSDate * _Nullable expirationDate;
        [NullAllowed, Export("expirationDate", ArgumentSemantic.Copy)]
        NSDate ExpirationDate { get; set; }

        // @property (nonatomic) BOOL isSubscriptionRenewable;
        [Export("isSubscriptionRenewable")]
        bool IsSubscriptionRenewable { get; set; }

        // @property (nonatomic) BOOL isFamilyShare;
        [Export("isFamilyShare")]
        bool IsFamilyShare { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable subscriptionRenewalProduct;
        [NullAllowed, Export("subscriptionRenewalProduct")]
        string SubscriptionRenewalProduct { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable subscriptionRenewalProductSku;
        [NullAllowed, Export("subscriptionRenewalProductSku")]
        string SubscriptionRenewalProductSku { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable subscriptionState;
        [NullAllowed, Export("subscriptionState")]
        string SubscriptionState { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable subscriptionPeriodType;
        [NullAllowed, Export("subscriptionPeriodType")]
        string SubscriptionPeriodType { get; set; }
    }

    // @interface IHBillingStatus : NSObject
    [BaseType(typeof(NSObject), Name = "IHBillingStatus")]
    [DisableDefaultCtor]
    interface IHBillingStatus
    {
        // @property (nonatomic, strong) IHError * _Nullable error;
        [NullAllowed, Export("error", ArgumentSemantic.Strong)]
        IHError Error { get; set; }

        // @property (nonatomic, copy) NSArray<NSString *> * _Nonnull filteredProductIds;
        [Export("filteredProductIds", ArgumentSemantic.Copy)]
        string[] FilteredProductIds { get; set; }
    }

    // @interface IHError : NSObject
    [BaseType(typeof(NSObject), Name = "IHError")]
    [DisableDefaultCtor]
    interface IHError
    {
        // @property (nonatomic, readonly, copy) NSString * _Nonnull message;
        [Export("message")]
        string Message { get; }

        // @property (nonatomic, readonly, copy) NSString * _Nonnull code;
        [Export("code")]
        string Code { get; }

        // @property (nonatomic, readonly, copy) NSString * _Nullable subcode;
        [NullAllowed, Export("subcode")]
        string Subcode { get; }

        // @property (nonatomic, readonly, copy) NSDictionary<NSString *, id> * _Nonnull params;
        [Export("params", ArgumentSemantic.Copy)]
        NSDictionary<NSString, NSObject> Params { get; }
    }

    // @interface IHProducts : NSObject
    [BaseType(typeof(NSObject), Name = "IHProducts")]
    [DisableDefaultCtor]
    interface IHProducts
    {
        // @property (nonatomic, copy) NSArray<IHActiveProduct *> * _Nonnull activeProducts;
        [Export("activeProducts", ArgumentSemantic.Copy)]
        IHActiveProduct[] ActiveProducts { get; set; }

        // @property (nonatomic, copy) NSArray<IHProduct *> * _Nonnull productsForSale;
        [Export("productsForSale", ArgumentSemantic.Copy)]
        IHProduct[] ProductsForSale { get; set; }
    }

    // @interface IHReceipt : NSObject
    [BaseType(typeof(NSObject), Name = "IHReceipt")]
    [DisableDefaultCtor]
    interface IHReceipt
    {
        // @property (nonatomic, copy) NSString * _Nonnull token;
        [Export("token")]
        string Token { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull sku;
        [Export("sku")]
        string Sku { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull context;
        [Export("context")]
        string Context { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull paymentProcessor;
        [Export("paymentProcessor")]
        string PaymentProcessor { get; set; }

        // @property (nonatomic) BOOL isFinished;
        [Export("isFinished")]
        bool IsFinished { get; set; }

        // @property (nonatomic, copy) NSDate * _Nullable processDate;
        [NullAllowed, Export("processDate", ArgumentSemantic.Copy)]
        NSDate ProcessDate { get; set; }
    }

    // @interface IHReceiptTransaction : IHActiveProduct
    [BaseType(typeof(IHActiveProduct), Name = "IHReceiptTransaction")]
    [DisableDefaultCtor]
    interface IHReceiptTransaction
    {
        // @property (nonatomic, copy) NSString * _Nullable webhookStatus;
        [NullAllowed, Export("webhookStatus")]
        string WebhookStatus { get; set; }

        // @property (nonatomic, copy) NSString * _Nullable user;
        [NullAllowed, Export("user")]
        string User { get; set; }
    }

    // @interface IHRestoreResponse : NSObject
    [BaseType(typeof(NSObject), Name = "IHRestoreResponse")]
    [DisableDefaultCtor]
    interface IHRestoreResponse
    {
        // @property (nonatomic, copy) NSArray<IHReceiptTransaction *> * _Nonnull newPurchases;
        [Export("newPurchases", ArgumentSemantic.Copy)]
        IHReceiptTransaction[] NewPurchases { get; set; }

        // @property (nonatomic, copy) NSArray<IHActiveProduct *> * _Nonnull transferredActiveProducts;
        [Export("transferredActiveProducts", ArgumentSemantic.Copy)]
        IHActiveProduct[] TransferredActiveProducts { get; set; }
    }

    // @interface IHSubscriptionIntroPhase : NSObject
    [BaseType(typeof(NSObject), Name = "IHSubscriptionIntroPhase")]
    [DisableDefaultCtor]
    interface IHSubscriptionIntroPhase
    {
        // @property (nonatomic, copy) NSString * _Nonnull type;
        [Export("type")]
        string Type { get; set; }

        // @property (nonatomic) double price;
        [Export("price")]
        double Price { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull currency;
        [Export("currency")]
        string Currency { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull localizedPrice;
        [Export("localizedPrice")]
        string LocalizedPrice { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull cycleDuration;
        [Export("cycleDuration")]
        string CycleDuration { get; set; }

        // @property (nonatomic) NSInteger cycleCount;
        [Export("cycleCount")]
        nint CycleCount { get; set; }

        // @property (nonatomic, copy) NSString * _Nonnull payment;
        [Export("payment")]
        string Payment { get; set; }
    }

    // @protocol IaphubDelegate
    [Protocol, Model]
    [BaseType(typeof(NSObject), Name = "IaphubDelegate")]
    interface IaphubDelegate
    {
        // - (void)didReceiveBuyRequestWithSku:(NSString * _Nonnull)sku;
        [Abstract]
        [Export("didReceiveBuyRequestWithSku:")]
        void DidReceiveBuyRequest(string sku);

        // - (void)didReceiveDeferredPurchaseWithTransaction:(IHReceiptTransaction * _Nonnull)transaction;
        [Abstract]
        [Export("didReceiveDeferredPurchaseWithTransaction:")]
        void DidReceiveDeferredPurchase(IHReceiptTransaction transaction);

        // - (void)didReceiveUserUpdate;
        [Abstract]
        [Export("didReceiveUserUpdate")]
        void DidReceiveUserUpdate();

        // - (void)didProcessReceiptWithErr:(IHError * _Nullable)err receipt:(IHReceipt * _Nullable)receipt;
        [Abstract]
        [Export("didProcessReceiptWithErr:receipt:")]
        void DidProcessReceipt([NullAllowed] IHError err, [NullAllowed] IHReceipt receipt);

        // - (void)didReceiveErrorWithErr:(IHError * _Nonnull)err;
        [Abstract]
        [Export("didReceiveErrorWithErr:")]
        void DidReceiveError(IHError err);
    }

    interface IIaphubDelegate { }

    // @interface Iaphub : NSObject
    [BaseType(typeof(NSObject), Name = "Iaphub")]
    [DisableDefaultCtor]
    interface Iaphub
    {
        // @property (nonatomic, class, weak) id <IaphubDelegate> _Nullable delegate;
        [Static]
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        IIaphubDelegate Delegate { get; set; }

        // + (void)startWithAppId:(NSString * _Nonnull)appId apiKey:(NSString * _Nonnull)apiKey userId:(NSString * _Nullable)userId allowAnonymousPurchase:(BOOL)allowAnonymousPurchase enableDeferredPurchaseListener:(BOOL)enableDeferredPurchaseListener enableStorekitV2:(BOOL)enableStorekitV2 environment:(NSString * _Nonnull)environment lang:(NSString * _Nonnull)lang sdk:(NSString * _Nonnull)sdk sdkVersion:(NSString * _Nonnull)sdkVersion;
        [Static]
        [Export("startWithAppId:apiKey:userId:allowAnonymousPurchase:enableDeferredPurchaseListener:enableStorekitV2:environment:lang:sdk:sdkVersion:")]
        void Start(string appId, string apiKey, [NullAllowed] string userId, bool allowAnonymousPurchase, bool enableDeferredPurchaseListener, bool enableStorekitV2, string environment, string lang, string sdk, string sdkVersion);

        // + (void)stop;
        [Static]
        [Export("stop")]
        void Stop();

        // + (BOOL)setLang:(NSString * _Nonnull)lang SWIFT_WARN_UNUSED_RESULT;
        [Static]
        [Export("setLang:")]
        bool SetLang(string lang);

        // + (void)loginWithUserId:(NSString * _Nonnull)userId :(void (^ _Nonnull)(IHError * _Nullable))completion;
        [Static]
        [Export("loginWithUserId::")]
        void Login(string userId, Action<IHError> completionHandler);

        // + (NSString * _Nullable)getUserId SWIFT_WARN_UNUSED_RESULT;
        [Static]
        [NullAllowed, Export("getUserId")]
        string GetUserId();

        // + (void)logout;
        [Static]
        [Export("logout")]
        void Logout();

        // + (void)setDeviceParamsWithParams:(NSDictionary<NSString *, NSString *> * _Nonnull)params;
        [Static]
        [Export("setDeviceParamsWithParams:")]
        void SetDeviceParams(NSDictionary deviceParams);

        // + (void)setUserTagsWithTags:(NSDictionary<NSString *, NSString *> * _Nonnull)tags :(void (^ _Nonnull)(IHError * _Nullable))completion;
        [Static]
        [Export("setUserTagsWithTags::")]
        void SetUserTags(NSDictionary tags, Action<IHError> completionHandler);

        // + (void)buyWithSku:(NSString * _Nonnull)sku crossPlatformConflict:(BOOL)crossPlatformConflict :(void (^ _Nonnull)(IHError * _Nullable, IHReceiptTransaction * _Nullable))completion;
        [Static]
        [Export("buyWithSku:crossPlatformConflict::")]
        void Buy(string sku, bool crossPlatformConflict, Action<IHError, IHReceiptTransaction> completionHandler);

        // + (void)restore:(void (^ _Nonnull)(IHError * _Nullable, IHRestoreResponse * _Nullable))completion;
        [Static]
        [Export("restore:")]
        void Restore(Action<IHError, IHRestoreResponse> completionHandler);

        // + (void)getActiveProductsWithIncludeSubscriptionStates:(NSArray<NSString *> * _Nonnull)includeSubscriptionStates :(void (^ _Nonnull)(IHError * _Nullable, NSArray<IHActiveProduct *> * _Nullable))completion;
        [Static]
        [Export("getActiveProductsWithIncludeSubscriptionStates::")]
        void GetActiveProducts(string[] includeSubscriptionStates, Action<IHError, IHActiveProduct[]> completionHandler);

        // + (void)getProductsForSale:(void (^ _Nonnull)(IHError * _Nullable, NSArray<IHProduct *> * _Nullable))completion;
        [Static]
        [Export("getProductsForSale:")]
        void GetProductsForSale(Action<IHError, IHProduct[]> completionHandler);

        // + (void)getProductsWithIncludeSubscriptionStates:(NSArray<NSString *> * _Nonnull)includeSubscriptionStates :(void (^ _Nonnull)(IHError * _Nullable, IHProducts * _Nullable))completion;
        [Static]
        [Export("getProductsWithIncludeSubscriptionStates::")]
        void GetProducts(string[] includeSubscriptionStates, Action<IHError, IHProducts> completionHandler);

        // + (IHBillingStatus * _Nonnull)getBillingStatus SWIFT_WARN_UNUSED_RESULT;
        [Static]
        [Export("getBillingStatus")]
        IHBillingStatus GetBillingStatus();

        // + (NSString * _Nonnull)getSDKVersion SWIFT_WARN_UNUSED_RESULT;
        [Static]
        [Export("getSDKVersion")]
        string GetSDKVersion();

        // + (void)showManageSubscriptions:(void (^ _Nonnull)(IHError * _Nullable))completion;
        [Static]
        [Export("showManageSubscriptions:")]
        void ShowManageSubscriptions(Action<IHError> completionHandler);

        // + (void)presentCodeRedemptionSheet:(void (^ _Nonnull)(IHError * _Nullable))completion;
        [Static]
        [Export("presentCodeRedemptionSheet:")]
        void PresentCodeRedemptionSheet(Action<IHError> completionHandler);
    }
}
