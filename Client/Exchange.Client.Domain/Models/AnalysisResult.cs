using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Exchange.Client.Domain.Models;

[ExcludeFromCodeCoverage]
public sealed class AnalysisResult
{
    public string Symbol { get; set; }
    public double AverageAskPrice { get; set; }
    public double AverageBidPrice { get; set; }
    public double StdAskDeviation { get; set; }
    public double StdBidDeviation { get; set; }
    public (double[] Ask, double[] Bid) Modes { get; set; }
    public (double Ask, double Bid) Median { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder($"Symbol {Symbol}\tAsk/Bid\n");

        sb.Append($"Average: {Math.Round(AverageAskPrice, 2)} / {Math.Round(AverageBidPrice, 2)}\n");
        sb.Append($"Standart dev.: {Math.Round(StdAskDeviation, 2)} / {Math.Round(StdBidDeviation, 2)}\n");
        sb.Append($"Medians: {Math.Round(Median.Ask, 2)} / {Math.Round(Median.Bid, 2)}");

        if (Modes.Ask.Length != 0 && Modes.Bid.Length != 0)
            sb.Append($"Modes: {Math.Round(Modes.Ask[0], 2)} / {Math.Round(Modes.Bid[0], 2)}\n");
        else
            sb.Append("Modes: Not enough data yet");

        return sb.ToString();
    }
}