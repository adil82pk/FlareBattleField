using Xunit;
using FlareBattleField.Classes;

namespace FlareBattleField.Test;

/// <summary>
/// Reference: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
/// </summary>
public class XCoordinateTests
{
    [Theory]
    // when board size is 0 then 0 coordinates are returned
    [InlineData(0, 0)]

    // when board size is -1 then 0 coordinates are returned
    [InlineData(-1, 0)]

    // when board size is 5 then 5 coordinates are returned
    [InlineData(5, 5)]

    // when board size is 10 then 10 coordinates are returned
    [InlineData(10, 10)]

    // when board size is 20 then 20 coordinates are returned
    [InlineData(20, 20)]
    public void InnitalizeXCoordinate_InputBoardSize_ReturnsDictionaryWithExpectedCount(int input, int expected)
    {
        var xCoordinates = XCoordinate.Initialize(input);
        Assert.Equal(expected, xCoordinates.Count);
    }
}