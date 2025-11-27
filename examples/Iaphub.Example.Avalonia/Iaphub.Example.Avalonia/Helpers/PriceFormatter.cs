using System.Collections.Generic;
using System.Text.RegularExpressions;
using Iaphub;

namespace Iaphub.Example.Avalonia.Helpers;

public static class PriceFormatter
{
    public static string FormatPrice(IaphubProduct product)
    {
        if (product == null)
        {
            return "";
        }

        var price = product.LocalizedPrice
                    ?? (product.Price.HasValue
                        ? $"{product.Price.Value:0.##} {product.Currency ?? string.Empty}".Trim()
                        : "Price not available");

        if (string.IsNullOrEmpty(product.SubscriptionDuration))
        {
            return price;
        }

        var duration = IsoToHuman(product.SubscriptionDuration);
        return string.IsNullOrEmpty(duration)
            ? price
            : $"{price} / {duration}";
    }

    private static string IsoToHuman(string isoDuration)
    {
        if (string.IsNullOrWhiteSpace(isoDuration)) return "";
        if (isoDuration == "P7D") isoDuration = "P1W";

        var match = Regex.Match(isoDuration, @"\d+");
        if (!match.Success) return "";

        var number = int.Parse(match.Value);
        var periods = new Dictionary<char, string>
        {
            ['D'] = "day",
            ['W'] = "week",
            ['M'] = "month",
            ['Y'] = "year"
        };

        var period = isoDuration[^1];
        if (!periods.TryGetValue(period, out var human)) return "";

        return number > 1 ? $"{number} {human}s" : human;
    }
}
