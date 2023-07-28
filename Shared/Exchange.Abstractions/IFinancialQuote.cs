namespace Exchange.Abstractions;

public interface IFinancialQuote
{
    int Id { get; }
    string Symbol { get; }
    double AskPrice { get; }
    double BidPrice { get; }
    long Time { get; }
}