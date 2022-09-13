namespace Server.Services.Generator;

internal sealed class QuoteSettings
{
    public string Group { get; set; }
    public string Symbol { get; set; }
    public decimal LowAsk { get; set; }
    public decimal HighAsk { get; set; }
    public decimal LowBid { get; set; }
    public decimal HighBid { get; set; }
}