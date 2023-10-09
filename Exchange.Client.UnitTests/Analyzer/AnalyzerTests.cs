using Exchange.Client.Application.Extensions;
using Exchange.Client.Application.Services;

namespace Exchange.Client.UnitTests.Analyzer;

public sealed partial class AnalyzerTests
{
    private readonly AnalyzerService _analyzer;

    public AnalyzerTests()
    {
        _analyzer = new AnalyzerService();
    }

    [Theory]
    [MemberData(nameof(CalculateAverageData))]
    public void CalculateMean_NormalValues_ReturnsExpected(double[] values, double expected)
    {
        var actual = values.CalculateMean();
        Assert.Equal(expected, actual);
    }
}