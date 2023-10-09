using Exchange.Abstractions;

namespace Exchange.Client.Infrastructure.Common.Interfaces;

public interface IQuotesCache
{
    void Add(IFinancialQuote quote);
    IDictionary<string, List<IFinancialQuote>> GetQuotes();
}