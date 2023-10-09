using Exchange.Abstractions;
using Exchange.Client.Infrastructure.Common.Interfaces;

namespace Exchange.Client.Infrastructure.Services;

public sealed class QuotesCache : IQuotesCache
{
    private readonly object _lock;
    private readonly Dictionary<string, List<IFinancialQuote>> _quotes;

    public QuotesCache()
    {
        _lock = new object();
        _quotes = new Dictionary<string, List<IFinancialQuote>>();
    }

    public void Add(IFinancialQuote quote)
    {
        lock (_lock)
        {
            if (_quotes.TryGetValue(quote.Symbol, out var value))
                value.Add(quote);
            else
                _quotes.Add(quote.Symbol, new List<IFinancialQuote>() { quote });
        }
    }

    public IDictionary<string, List<IFinancialQuote>> GetQuotes()
    {
        lock (_lock)
        {
            return _quotes;
        }
    }
}