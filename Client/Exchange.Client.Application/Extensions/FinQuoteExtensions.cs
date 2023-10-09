using Exchange.Abstractions;
using Exchange.Client.Domain.Models;

namespace Exchange.Client.Application.Extensions;

public static class FinQuoteExtensions
{
    public static AnalysisResult AnalyzeQuote(this IFinancialQuote[] quote, string symbol)
    {
        var askPrices = quote.Select(p => p.AskPrice).ToArray();
        var bidPrices = quote.Select(p => p.BidPrice).ToArray();

        return new AnalysisResult()
        {
            AverageAskPrice = askPrices.CalculateMean(),
            AverageBidPrice = bidPrices.CalculateMean(),
            StdAskDeviation = askPrices.CalculateStdDeviation(),
            StdBidDeviation = bidPrices.CalculateStdDeviation(),
            Modes = (askPrices.CalculateMode(), bidPrices.CalculateMode()),
            Median = (askPrices.CalculateMedian(), bidPrices.CalculateMedian()),
            Symbol = symbol
        };
    }
}