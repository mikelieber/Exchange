using System.Diagnostics.CodeAnalysis;

namespace Exchange.Server.Domain.Models;

[ExcludeFromCodeCoverage]
public sealed class QuoteSetting
{
    public string Group { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public double LowAsk { get; set; }
    public double HighAsk { get; set; }
    public double LowBid { get; set; }
    public double HighBid { get; set; }
}