using Exchange.Abstractions;
using Exchange.Models;
using Exchange.Server.Application.Common.Interfaces;
using Exchange.Server.Domain.Models;
using Microsoft.Extensions.Options;

namespace Exchange.Server.Infrastructure;

public class QuoteGeneratorService : IQuotesGeneratorService
{
    private readonly QuoteSetting[] _quoteSettings;

    public QuoteGeneratorService(IOptions<QuoteGeneratorOptions> options)
    {
        _quoteSettings = options.Value.Settings;
    }

    public ValueTask<IDictionary<string, IFinancialQuote>> GenerateQuotesAsync(CancellationToken ct)
    {
        decimal NextDecimal(decimal min, decimal max)
        {
            return (decimal)Random.Shared.NextDouble() * (max - min) + min;
        }

        IDictionary<string, IFinancialQuote> result = new Dictionary<string, IFinancialQuote>(_quoteSettings.Length);

        for (var i = 0; i < _quoteSettings.Length; i++)
        {
            ct.ThrowIfCancellationRequested();
            var quoteSettings = _quoteSettings[i];
            var quote = new FinancialQuote()
            {
                Symbol = quoteSettings.Symbol,
                AskPrice = NextDecimal(quoteSettings.LowAsk, quoteSettings.HighAsk),
                BidPrice = NextDecimal(quoteSettings.LowBid, quoteSettings.HighBid),
                Time = DateTime.Now
            };
            result.Add(quoteSettings.Group, quote);
        }

        return ValueTask.FromResult(result);
    }
}