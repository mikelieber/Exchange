using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Exchange.Abstractions;

namespace Exchange.Models;

[ExcludeFromCodeCoverage]
[DebuggerDisplay("Symbol: {Symbol} Ask: {AskPrice} Bid {BidPrice}")]
public sealed class FinancialQuote : IFinancialQuote
{
    [Key]
    public int Id { get; set; }
    public string Symbol { get; set; }
    public double AskPrice { get; set; }
    public double BidPrice { get; set; }
    public long Time { get; set; }

    public override string ToString()
    {
        return $"Symbol: {Symbol}\tAsk: {Math.Round(AskPrice, 2)}\tBid {Math.Round(BidPrice, 2)}";
    }

    public static FinancialQuote Create(IFinancialQuote source)
    {
        return new FinancialQuote
        {
            BidPrice = source.BidPrice,
            Id = source.Id,
            Symbol = source.Symbol,
            Time = source.Time,
            AskPrice = source.AskPrice
        };
    }
}