using CommunityToolkit.Mvvm.ComponentModel;

namespace Iaphub.Example.Shared.Models;

public partial class ProductViewModel : ObservableObject
{
    public required global::Iaphub.IaphubProduct Product { get; init; }

    [ObservableProperty]
    private bool isProcessing;

    public string FormattedPrice
    {
        get
        {
            if (!string.IsNullOrEmpty(Product.LocalizedPrice))
            {
                return Product.LocalizedPrice;
            }

            if (Product.Price.HasValue && !string.IsNullOrEmpty(Product.Currency))
            {
                return $"{Product.Price.Value} {Product.Currency}";
            }

            return string.Empty;
        }
    }
}
