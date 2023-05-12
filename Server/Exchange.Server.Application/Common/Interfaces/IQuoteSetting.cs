namespace Exchange.Server.Application.Common.Interfaces;

public interface IQuoteSetting
{
    public string Group { get; set; }
    public string Symbol { get; set; }
    public decimal LowAsk { get; set; }
    public decimal HighAsk { get; set; }
    public decimal LowBid { get; set; }
    public decimal HighBid { get; set; }
}