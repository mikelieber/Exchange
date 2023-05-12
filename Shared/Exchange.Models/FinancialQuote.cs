using System.ComponentModel.DataAnnotations;
using Exchange.Abstractions;

namespace Exchange.Models;

public sealed class FinancialQuote : IFinancialQuote
{
    [Key]
    public int Id { get; set; }

    public string Symbol { get; set; }
    public decimal AskPrice { get; set; }
    public decimal BidPrice { get; set; }
    public DateTime Time { get; set; }

    public override string ToString()
    {
        return $"Symbol: {Symbol}\tAsk: {Math.Round(AskPrice, 2)}\tBid {Math.Round(BidPrice, 2)}";
    }

    public static FinancialQuote Create(IFinancialQuote source)
    {
        return new FinancialQuote()
        {
            BidPrice = source.BidPrice,
            Id = source.Id,
            Symbol = source.Symbol,
            Time = source.Time,
            AskPrice = source.AskPrice
        };
    }
}