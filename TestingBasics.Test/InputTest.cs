using Xunit;
using FlareBattleField.Classes;
using FlareBattleShip.Classes;

namespace FlareBattleField.Test;

/// <summary>
/// Reference: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
/// </summary>
public class InputTest
{
    [Theory]
    // A is the 1st character which is X and Y is 6
    [InlineData("a6", 1, 6)]
    [InlineData("A6", 1, 6)]
    // B is the 2nd character which is X and Y is 2
    [InlineData("B2", 2, 2)]
    // J is the 10th character which is X and Y is 4
    [InlineData("J4", 10, 4)]
    [InlineData("J8", 10, 8)]
    // J is the 20th character which is X and Y is 20
    [InlineData("J20", 10, 20)]
    // T is the 20th character which is X and Y is 20
    [InlineData("T20", 20, 20)]
    // U is the 21st character and not in the dictionary
    [InlineData("U20", -1, -1)]
    public void InnitalizeXCoordinate_BoardSizeTen_ReturnsDictionaryWithCountTen(string input, int expectedX, int expectedY)
    {
        // Initialize dictionary with 20 items.
        var xCoordinates = XCoordinate.Initialize(20);
        var test = Input.Analyze(input, xCoordinates);
        Assert.Equal(test.X == expectedX, test.Y == expectedY);
    }
}