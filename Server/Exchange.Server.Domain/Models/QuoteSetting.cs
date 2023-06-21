namespace Exchange.Server.Domain.Models;

public sealed class QuoteSetting
{
    public string Group { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public decimal LowAsk { get; set; }
    public decimal HighAsk { get; set; }
    public decimal LowBid { get; set; }
    public decimal HighBid { get; set; }
}