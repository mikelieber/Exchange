using Microsoft.Extensions.Options;
using Models;

namespace Server.Services.Generator;

internal sealed class FinancialQuoteGeneratorService
{
    private readonly QuoteSettings[] _quoteSettings;

    public FinancialQuoteGeneratorService(IOptions<QuoteGeneratorOptions> options)
    {
        _quoteSettings = options.Value.Settings;
    }

    internal async Task<IDictionary<string, FinancialQuote>> GenerateQuotesAsync(CancellationToken ct)
    {
        decimal NextDecimal(decimal min, decimal max) => (decimal)Random.Shared.NextDouble() * (max - min) + min;
        IDictionary<string, FinancialQuote> result = new Dictionary<string, FinancialQuote>(_quoteSettings.Length);

        for (int i = 0; i < _quoteSettings.Length; i++)
        {
            ct.ThrowIfCancellationRequested();
            QuoteSettings quoteSettings = _quoteSettings[i];
            FinancialQuote quote = new()
            {
                Symbol = quoteSettings.Symbol,
                AskPrice = NextDecimal(quoteSettings.LowAsk, quoteSettings.HighAsk),
                BidPrice = NextDecimal(quoteSettings.LowBid, quoteSettings.HighBid),
                Time = DateTime.Now
            };
            result.Add(quoteSettings.Group, quote);
        }

        return await Task.FromResult(result);
    }
}