using Exchange.Abstractions;
using Exchange.Server.Application.Common.Interfaces;

namespace Exchange.Server.Infrastructure.Storage;

public sealed class QuotesStorage : IQuotesStorage
{
    private readonly Dictionary<string, IFinancialQuote> _quotes;

    public QuotesStorage()
    {
        _quotes = new Dictionary<string, IFinancialQuote>();
    }

    public Dictionary<string, IFinancialQuote> GetQuotes()
    {
        return _quotes;
    }

    public void InsertOrUpdateQuote(string group, IFinancialQuote? quote)
    {
        if (quote == null)
            return;

        _quotes[group] = quote;
    }
}