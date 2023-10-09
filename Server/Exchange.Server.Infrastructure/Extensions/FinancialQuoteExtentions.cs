using System.Text;
using System.Text.Json;
using Exchange.Abstractions;
using Exchange.Models;
using Exchange.Server.Domain.Models;

namespace Exchange.Server.Infrastructure.Extensions;

public static class FinancialQuoteExtensions
{
    public static byte[] SerializeToBytes(this IFinancialQuote quote)
    {
        var json = JsonSerializer.Serialize(quote);
        return Encoding.UTF8.GetBytes(json);
    }

    public static FinancialQuote GenerateFromSetting(QuoteSetting setting)
    {
        return new FinancialQuote
        {
            Symbol = setting.Symbol,
            AskPrice = NextDecimal(setting.LowAsk, setting.HighAsk),
            BidPrice = NextDecimal(setting.LowBid, setting.HighBid),
            Time = DateTime.Now.Ticks
        };
    }

    private static double NextDecimal(double min, double max)
    {
        return Random.Shared.NextDouble() * (max - min) + min;
    }
}