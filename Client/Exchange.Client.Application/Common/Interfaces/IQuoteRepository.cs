using Exchange.Abstractions;

namespace Exchange.Client.Application.Common.Interfaces;

public interface IQuoteRepository
{
    IEnumerable<IFinancialQuote> GetQuotes();
    IFinancialQuote? GetById(int id);
    void InsertQuote(IFinancialQuote quote);
    void UpdateQuote(IFinancialQuote quote);
    void DeleteQuote(int id);
    void Save();
}