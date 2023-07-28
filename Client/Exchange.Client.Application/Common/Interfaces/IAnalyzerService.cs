using Exchange.Abstractions;
using Exchange.Client.Domain.Models;

namespace Exchange.Client.Application.Common.Interfaces;

public interface IAnalyzerService
{
    IEnumerable<AnalysisResult> Analyze(IDictionary<string, List<IFinancialQuote>> quotes);
}