using Client.Models;
using Models;

namespace Client.Services.Analyzer;

internal sealed class AnalyzerService
{
    public async Task<AnalysisResult[]> AnalyzeAsync(Dictionary<string, List<FinancialQuote>> quotes, CancellationToken ct)
    {
        var results = new AnalysisResult[quotes.Count];
        
        int index = 0;
        Parallel.ForEach(quotes, async (pair) =>
        {
            results[index++] = new AnalysisResult()
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

        for (int i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askSum += quote.AskPrice;
            bidSum += quote.BidPrice;
        }

        return await Task.FromResult((askSum / count, bidSum / count));
    }

    private async Task<(decimal, decimal)> CalculateStandartDeviationAsync(List<FinancialQuote> quotes, CancellationToken ct)
    {
        decimal CalculateMean(decimal[] values)
        {
            var temp = 0.0m;
            for (int i = 0; i < values.Length; i++)
                temp += values[i];
            
            return temp / values.Length;
        }

        decimal Power(decimal value) => value * value;

        decimal GetSqrt(decimal value)
        {
            var temp = (double)value;
            return (decimal)Math.Sqrt(temp);
        }

        decimal Sum(decimal[] values)
        {
            var temp = 0.0m;
            for (int i = 0; i < values.Length; i++)
                temp += values[i];
            return temp;
        }

        var count = quotes.Count;
        decimal[] askValues = new decimal[count];
        decimal[] bidValues = new decimal[count];
        for (int i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askValues[i] = quote.AskPrice;
            bidValues[i] = quote.BidPrice;
        }

        decimal[] askSquareDev = new decimal[count];
        decimal[] bidSquareDev = new decimal[count];
        for (int i = 0; i < count; i++)
        {
            askSquareDev[i] = Power(askValues[i] - CalculateMean(askValues));
            bidSquareDev[i] = Power(bidValues[i] - CalculateMean(bidValues));
        }

        decimal askSquareSum = Sum(askSquareDev);
        decimal bidSquareSum = Sum(bidSquareDev);

        decimal askStdDev = GetSqrt(askSquareSum / count);
        decimal bidStdDev = GetSqrt(bidSquareSum / count);

        return await Task.FromResult((askStdDev, bidStdDev));
    }

    private async Task<(decimal[], decimal[])> CalculateModeAsync(List<FinancialQuote> quotes, CancellationToken ct)
    {
        List<decimal> CalculateMode(decimal[] values)
        {
            decimal[] sorted = new decimal[values.Length];
            values.CopyTo(sorted, 0);
            Array.Sort(sorted);

            List<decimal> result = new ();
            var counts = new Dictionary<decimal, int>();
            int max = 0;
            foreach (var num in sorted)
            {
                if (counts.ContainsKey(num))
                    counts[num] = counts[num] + 1;
                else
                    counts[num] = 1;
            }

            foreach (var key in counts.Keys)
            {
                if (counts[key] > max)
                {
                    max = counts[key];
                    result.Add(max);
                }
            }

            return result;
        }

        var count = quotes.Count;
        decimal[] askValues = new decimal[count];
        decimal[] bidValues = new decimal[count];
        for (int i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askValues[i] = quote.AskPrice;
            bidValues[i] = quote.BidPrice;
        }

        decimal[] askModes = CalculateMode(askValues).ToArray();
        decimal[] bidModes = CalculateMode(bidValues).ToArray();

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
        decimal[] askValues = new decimal[count];
        decimal[] bidValues = new decimal[count];
        for (int i = 0; i < count; i++)
        {
            var quote = quotes[i];
            askValues[i] = quote.AskPrice;
            bidValues[i] = quote.BidPrice;
        }

        return await Task.FromResult((Median(askValues), Median(bidValues)));
    }
}