using Client.Models;
using Exchange.Domain.Models;

namespace Client.Services.Analyzer;

internal sealed class AnalyzerService
{
    public async Task<AnalysisResult[]> AnalyzeAsync(Dictionary<string, List<FinancialQuote>> quotes,
        CancellationToken ct)
    {
        var results = new AnalysisResult[quotes.Count];

        var index = 0;
        Parallel.ForEach(quotes, async pair =>
        {
            results[index++] = new AnalysisResult
            {
                Average = await CalculateAverageAsync(pair.Value, ct),
                StandartDeviation = await CalculateStandartDeviationAsync(pair.Value, ct),
                Modes = await CalculateModeAsync(pair.Value, ct),
                Median = await CalculateMedianAsync(pair.Value, ct),
                Symbol = pair.Key
            };
        });

        return await Task.FromResult(results);
    }

    private async Task<(decimal, decimal)> CalculateAverageAsync(List<FinancialQuote> quotes, CancellationToken ct)
    {
        var askSum = 0.0m;
        var bidSum = 0.0m;
        var count = quotes.Count;

        for (var i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askSum += quote.AskPrice;
            bidSum += quote.BidPrice;
        }

        return await Task.FromResult((askSum / count, bidSum / count));
    }

    private async Task<(decimal, decimal)> CalculateStandartDeviationAsync(List<FinancialQuote> quotes,
        CancellationToken ct)
    {
        decimal CalculateMean(decimal[] values)
        {
            var temp = 0.0m;
            for (var i = 0; i < values.Length; i++)
                temp += values[i];

            return temp / values.Length;
        }

        decimal Power(decimal value)
        {
            return value * value;
        }

        decimal GetSqrt(decimal value)
        {
            var temp = (double)value;
            return (decimal)Math.Sqrt(temp);
        }

        decimal Sum(decimal[] values)
        {
            var temp = 0.0m;
            for (var i = 0; i < values.Length; i++)
                temp += values[i];
            return temp;
        }

        var count = quotes.Count;
        var askValues = new decimal[count];
        var bidValues = new decimal[count];
        for (var i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askValues[i] = quote.AskPrice;
            bidValues[i] = quote.BidPrice;
        }

        var askSquareDev = new decimal[count];
        var bidSquareDev = new decimal[count];
        for (var i = 0; i < count; i++)
        {
            askSquareDev[i] = Power(askValues[i] - CalculateMean(askValues));
            bidSquareDev[i] = Power(bidValues[i] - CalculateMean(bidValues));
        }

        var askSquareSum = Sum(askSquareDev);
        var bidSquareSum = Sum(bidSquareDev);

        var askStdDev = GetSqrt(askSquareSum / count);
        var bidStdDev = GetSqrt(bidSquareSum / count);

        return await Task.FromResult((askStdDev, bidStdDev));
    }

    private async Task<(decimal[], decimal[])> CalculateModeAsync(List<FinancialQuote> quotes, CancellationToken ct)
    {
        List<decimal> CalculateMode(decimal[] values)
        {
            var sorted = new decimal[values.Length];
            values.CopyTo(sorted, 0);
            Array.Sort(sorted);

            List<decimal> result = new();
            var counts = new Dictionary<decimal, int>();
            var max = 0;
            foreach (var num in sorted)
                if (counts.ContainsKey(num))
                    counts[num] = counts[num] + 1;
                else
                    counts[num] = 1;

            foreach (var key in counts.Keys)
                if (counts[key] > max)
                {
                    max = counts[key];
                    result.Add(max);
                }

            return result;
        }

        var count = quotes.Count;
        var askValues = new decimal[count];
        var bidValues = new decimal[count];
        for (var i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askValues[i] = quote.AskPrice;
            bidValues[i] = quote.BidPrice;
        }

        var askModes = CalculateMode(askValues).ToArray();
        var bidModes = CalculateMode(bidValues).ToArray();

        return await Task.FromResult((askModes, bidModes));
    }

    private async Task<(decimal, decimal)> CalculateMedianAsync(List<FinancialQuote> quotes, CancellationToken ct)
    {
        decimal Median(decimal[] xs)
        {
            Array.Sort(xs);
            return xs[xs.Length / 2];
        }

        var count = quotes.Count;
        var askValues = new decimal[count];
        var bidValues = new decimal[count];
        for (var i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askValues[i] = quote.AskPrice;
            bidValues[i] = quote.BidPrice;
        }

        return await Task.FromResult((Median(askValues), Median(bidValues)));
    }
}