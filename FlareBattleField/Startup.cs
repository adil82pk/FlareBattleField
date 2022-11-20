using Microsoft.Extensions.Configuration;
using FlareBattleShip.Interfaces;
using FlareBattleShip.Classes;
using Microsoft.Extensions.DependencyInjection;

namespace FlareBattleField
{
    /// <summary>
    /// Startup Class.
    /// </summary>
    public static class Startup
    {

        /// <summary>
        /// Setup configuration 
        /// </summary>
        /// <param name="args">Args</param>
        /// <returns>IConfiguration</returns>
        public static IConfiguration SetupConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        /// <summary>
        /// Register Services 
        /// </summary>
        /// <param name="args">Args.</param>
        /// <param name="numberOfShips">Number of ships.</param>
        /// <returns>ServiceProvider object.</returns>
        public static ServiceProvider RegisterServices(string[] args, int numberOfShips)
        {
            IConfiguration configuration = SetupConfiguration(args);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IBattleShipGame>(x => new BattleShipGame(numberOfShips, int.Parse(configuration.GetSection("AppConfig:BoardSize").Value ?? "0".ToString())));
            serviceCollection.AddSingleton(configuration);
            return serviceCollection.BuildServiceProvider();
        }
    }
}
