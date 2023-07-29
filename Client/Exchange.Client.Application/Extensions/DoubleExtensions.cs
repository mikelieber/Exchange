namespace Exchange.Client.Application.Extensions;

public static class DoubleExtensions
{
    public static double CalculateMean(this double[]? values)
    {
        var count = 0;
        var sum = 0.0;

        if (values == null)
            return sum;

        foreach (var value in values)
        {
            sum += value;
            count++;
        }

        if (count == 0)
            return sum;

        return sum / count;
    }

    public static double CalculateMedian(this double[]? values)
    {
        switch (values)
        {
            case null:
                return 0;
            case { Length: 0 }:
                return 0;
            case { Length: 1 }:
                return values[0];
            case { Length: 2 }:
                return values.CalculateMean();
            default:
                values.SortBubble();
                break;
        }

        var midPoint = values.Length / 2;
        return values.Length % 2 == 0
            ? new[] { values[midPoint], values[--midPoint] }.CalculateMean()
            : values[midPoint];
    }

    public static double[] CalculateMode(this double[] values)
    {
        switch (values)
        {
            case null:
                return Array.Empty<double>();
            case { Length: 0 }:
                return Array.Empty<double>();
            case { Length: 1 }:
                return new [] { values[0] };
        }

        var counters = new Dictionary<double, int>();
        foreach (var value in values)
        {
            var rounded = Math.Round(value, 2);
            if (counters.ContainsKey(rounded))
                counters[rounded]++;
            else
                counters[rounded] = 1;
        }

        var result = new List<double>();
        foreach (var value in counters.Keys)
        {
            var counter = counters[value];
            if (counter > 1)
                result.Add(value);
        }

        return result.ToArray();
    }

    public static double SumArray(this double[] values)
    {
        switch (values)
        {
            case null:
                return 0;
            case { Length: 0 }:
                return 0;
        }

        var temp = values[0];
        for (var i = 1; i < values.Length; i++)
            temp += values[i];

        return temp;
    }

    public static double CalculateStdDeviation(this double[] values)
    {
        switch (values)
        {
            case null:
                return 0;
            case { Length: 0 }:
                return 0;
            case { Length: 1}:
                return 0;
        }

        var mean = values.CalculateMean();
        var deviations = new double[values.Length];
        for (var i = 0; i < values.Length; i++)
        {
            var distance = values[i] - mean;
            deviations[i] = distance * distance;
        }

        return Math.Sqrt(deviations.CalculateMean());
    }

    public static void SortBubble(this double[] values)
    {
        for (var i = 0; i < values.Length - 1; i++)
            for (var j = 0; j < values.Length - i - 1; j++)
                if (values[j] > values[j + 1])
                    (values[j], values[j + 1]) = (values[j + 1], values[j]);
    }
}