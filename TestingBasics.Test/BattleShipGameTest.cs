using Xunit;
using Microsoft.Extensions.Configuration;
using FlareBattleShip.Interfaces;
using FlareBattleShip.Classes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FlareBattleField.Test;

/// <summary>
/// Reference: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
/// </summary>
public class BattleShipGameTest
{

    [Theory]
    // inputBoardSize, battleShips, expectedBattleShips
    [InlineData("10", 0, 0)]
    [InlineData("20", 10, 10)]
    [InlineData("30", 15, 15)]
    public void InnitalizePosition_TestInputsProvided_ReturnsExpectedShipsCount(string inputBoardSize, int battleShips, int expectedBattleShips)
    {
        //Arrange
        var serviceProvider = InitializeDependencies(inputBoardSize, battleShips);
        var myBattleShipGame = serviceProvider.GetService<IBattleShipGame>();

        Assert.Equal(myBattleShipGame?.Ship.Count, expectedBattleShips);
    }

    private ServiceProvider InitializeDependencies(string inputBoardSize, int battleShips)
    {
        // using in memory settings so that we do not have dependency on actual appsetting.json file.
        var inMemorySettings = new Dictionary<string, string> { { "AppConfig:BoardSize", inputBoardSize } };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IBattleShipGame>(x => new BattleShipGame(battleShips, int.Parse(configuration.GetSection("AppConfig:BoardSize").Value ?? "0".ToString())));
        return serviceCollection.BuildServiceProvider();
    }
}