using Exchange.Abstractions;
using Exchange.Server.Application.Common.Interfaces;
using Exchange.Server.Domain.Models;
using Exchange.Server.Infrastructure.Extensions;
using Microsoft.Extensions.Options;

namespace Exchange.Server.Infrastructure.Services;

public class QuoteGeneratorService : IQuotesGeneratorService
{
    private readonly QuoteSetting[] _quoteSettings;

    public QuoteGeneratorService(IOptions<QuoteGeneratorOptions> options)
    {
        _quoteSettings = options.Value.Settings;
    }

    public Dictionary<string, IFinancialQuote> GenerateQuotes()
    {
        var result = new Dictionary<string, IFinancialQuote>(_quoteSettings.Length);

        foreach (var quoteSettings in _quoteSettings)
        {
            var quote = FinancialQuoteExtensions.GenerateFromSetting(quoteSettings);
            result.Add(quoteSettings.Group, quote);
        }

        return result;
    }
}