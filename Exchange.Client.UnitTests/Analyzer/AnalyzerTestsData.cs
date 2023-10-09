namespace Exchange.Client.UnitTests.Analyzer;

public sealed partial class AnalyzerTests
{
    public static object[][] CalculateAverageData = new[]
    {
        new object[]
        {
            new []
            {
                1.5,
                1.7,
                1.8
            },
            (1.5 + 1.7 + 1.8) / 3
        },
        new object[]
        {
            null,
            0
        },
        new object[]
        {
            new double[] {},
            0
        }
    };
}