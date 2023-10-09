using Exchange.Abstractions;
using Exchange.Client.Application.Common.Interfaces;
using Exchange.Client.Application.Extensions;
using Exchange.Client.Domain.Models;

namespace Exchange.Client.Application.Services;

public sealed class AnalyzerService : IAnalyzerService
{
    public IEnumerable<AnalysisResult> Analyze(IDictionary<string, List<IFinancialQuote>> quotes)
    {
        var results = new AnalysisResult[quotes.Count];

        var index = 0;
        Parallel.ForEach(quotes, pair => {
            results[index++] = pair.Value.ToArray().AnalyzeQuote(pair.Key);
        });

        return results;
    }
}