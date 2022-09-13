namespace Models;

public class FinancialQuote
{
    public string Symbol { get; set; }
    public decimal AskPrice { get; set; }
    public decimal BidPrice { get; set; }
    public DateTime Time { get; set; }

    public override string ToString()
    {
        return $"Symbol: {Symbol}\tAsk: {Math.Round(AskPrice, 2)}\tBid {Math.Round(BidPrice, 2)}";
    }
}