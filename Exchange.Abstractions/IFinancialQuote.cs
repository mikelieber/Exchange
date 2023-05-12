namespace Exchange.Abstractions;

public interface IFinancialQuote
{
    int Id { get; }
    string Symbol { get; }
    decimal AskPrice { get; }
    decimal BidPrice { get; }
    DateTime Time { get; }
}