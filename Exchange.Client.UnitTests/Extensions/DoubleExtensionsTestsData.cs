namespace Exchange.Client.UnitTests.Extensions;

public sealed partial class DoubleExtensionsTests
{
    public static object[][] CalculateMedianData = new[]
    {
        new object[]
        {
            new []
            {
                1.7,
                1.8,
                1.5
            },
            1.7
        },
        new object[]
        {
            new []
            {
                1.8,
                2.0,
                1.7,
                1.5
            },
            (1.7 + 1.8) / 2
        },
        new object[]
        {
            new []
            {
                2.0,
                1.7
            },
            (2.0 + 1.7) / 2
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
        },
        new object[]
        {
            new [] { 1.2 },
            1.2
        }
    };

    public static object[][] CalculateModeData = new[]
    {
        new object[]
        {
            new []
            {
                1.7,
                1.8,
                1.5,
                1.7
            },
            new [] { 1.7 },
            1
        },
        new object[]
        {
            null,
            new double[] {},
            0
        },
        new object[]
        {
            new double[] {},
            new double[] {},
            0
        },
        new object[]
        {
            new [] { 1.2 },
            new [] { 1.2 },
            1
        },
        new object[]
        {
            new [] { 1.3, 1.3 },
            new [] { 1.3 },
            1
        },
        new object[]
        {
            new [] { 1.3, 0.1, 0.5, 1.3, 1.3 },
            new [] { 1.3 },
            1
        },
        new object[]
        {
            new [] { 1.9, 1.3, 0.5, 0.5, 0.5, 1.3, 1.3, 2.0 },
            new [] { 1.3, 0.5 },
            2
        }
    };

    public static object[][] SumArrayData = new[]
    {
        new object[]
        {
            new []
            {
                1.7,
                1.8,
                1.5
            },
            (1.7 + 1.8 + 1.5)
        },
        new object[]
        {
            new []
            {
                1.8
            },
            1.8
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

    public static object[][] CalculateStdDeviationData = new[]
    {
        new object[]
        {
            null,
            0
        },
        new object[]
        {
            new double[] {},
            0
        },
        new object[]
        {
            new []
            {
                6.0, 2, 3, 1
            },
            1.87
        },
        new object[]
        {
            new [] { 6.0 },
            0
        },
        new object[]
        {
            new [] { 6.0, 7.0 },
            0.5
        }
    };
}