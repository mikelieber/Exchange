using Exchange.Abstractions;

namespace Exchange.Server.Application.Common.Interfaces;

public interface IQuotesGeneratorService
{
    Dictionary<string, IFinancialQuote> GenerateQuotes();
}