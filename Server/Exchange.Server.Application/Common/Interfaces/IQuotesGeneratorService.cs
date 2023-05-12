using Exchange.Abstractions;

namespace Exchange.Server.Application.Common.Interfaces;

public interface IQuotesGeneratorService
{
    ValueTask<IDictionary<string, IFinancialQuote>> GenerateQuotesAsync(CancellationToken ct);
}