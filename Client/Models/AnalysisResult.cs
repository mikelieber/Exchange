namespace Client.Models;

internal sealed class AnalysisResult
{
    public string Symbol { get; set; }
    public (decimal Ask, decimal Bid) Average { get; set; }
    public (decimal Ask, decimal Bid) StandartDeviation { get; set; }
    public (decimal[] Ask, decimal[] Bid) Modes { get; set; }
    public (decimal Ask, decimal Bid) Median { get; set; }

    public override string ToString()
    {
        return $"Symbol {Symbol}\tAsk/Bid\n" +
               $"Average: {Math.Round(Average.Ask, 2)} / {Math.Round(Average.Bid, 2)}\n" +
               $"Standart dev.: {Math.Round(StandartDeviation.Ask, 2)} / {Math.Round(StandartDeviation.Bid, 2)}\n" +
               $"Modes: {Math.Round(Modes.Ask[0], 2)} / {Math.Round(Modes.Bid[0], 2)}\n" +
               $"Medians: {Math.Round(Median.Ask, 2)} / {Math.Round(Median.Bid, 2)}";
    }
}