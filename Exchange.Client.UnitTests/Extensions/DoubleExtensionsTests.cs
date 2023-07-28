using Exchange.Client.Application.Extensions;

namespace Exchange.Client.UnitTests.Extensions;

public sealed partial class DoubleExtensionsTests
{
    [Theory]
    [MemberData(nameof(CalculateMedianData))]
    public void CalculateMedian_ArrayOrValues_ReturnsExpected(double[] values, double expected)
    {
        Assert.Equal(expected, values.CalculateMedian());
    }

    [Theory]
    [MemberData(nameof(CalculateModeData))]
    public void CalculateMode_ArrayOrValues_ReturnsExpected(double[] values, double[] expected, int expectedLength)
    {
        Assert.Multiple(() =>
        {
            var result = values.CalculateMode();
            Assert.Equal(expected, result);
            Assert.Equal(expectedLength, result.Length);
        });
    }

    [Theory]
    [MemberData(nameof(CalculateStdDeviationData))]
    public void CalculateStdDeviation_ArrayOrValues_ReturnsExpected(double[] values, double expected)
    {
        var roundExpected = Math.Round(expected, 2);
        var roundResult = Math.Round(values.CalculateStdDeviation(), 2);
        Assert.Equal(roundExpected, roundResult);
    }

    [Theory]
    [MemberData(nameof(SumArrayData))]
    public void SumArray_ArrayOrValues_ReturnsExpected(double[] values, double expected)
    {
        Assert.Equal(expected, values.SumArray());
    }

    [Fact]
    public void SortBubble_NormalValues_ReturnsSorted()
    {
        var values = new[] { 10.1, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        var expected = values[0];
        values.SortBubble();

        Assert.Equal(expected, values[9]);
    }
}