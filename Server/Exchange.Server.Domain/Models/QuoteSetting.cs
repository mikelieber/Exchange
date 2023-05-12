namespace Exchange.Server.Domain.Models;

public sealed class QuoteSetting
{
    public string Group { get; set; }
    public string Symbol { get; set; }
    public decimal LowAsk { get; set; }
    public decimal HighAsk { get; set; }
    public decimal LowBid { get; set; }
    public decimal HighBid { get; set; }
}