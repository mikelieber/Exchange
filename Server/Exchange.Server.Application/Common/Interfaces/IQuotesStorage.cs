using Exchange.Abstractions;

namespace Exchange.Server.Application.Common.Interfaces;

public interface IQuotesStorage
{
    public Dictionary<string, IFinancialQuote> GetQuotes();
    public void InsertOrUpdateQuote(string group, IFinancialQuote? quote);
}