using FlareBattleShip.Models;
using FlareBattleShip.Interfaces;
using FlareBattleShip.Classes;
using static System.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FlareBattleField;
using FlareBattleField.Classes;

namespace FlareBattleShip
{
    class Program
    {
        static string[,]? myBoard;
        static string[,]? opponentBoard;
        static int boardSize = 10;

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Array of args.</param>
        static void Main(string[] args)
        {
            bool displayShips = false;

            WriteLine("Enter number of battleships (5 maximum) to play:");
            string input = ReadLine();

            if (int.TryParse(input, out int value) && value > 0 && value <= 5)
            {
                ServiceProvider serviceProvider = Startup.RegisterServices(args, value);
                var configuration = serviceProvider.GetService<IConfiguration>();

                // Keeping board size it to 10. Can be changed from the appSetting file max upto 99.
                boardSize = int.Parse(configuration.GetSection("AppConfig:BoardSize").Value ?? "0".ToString());

                // Setup DI
                var myBattleShipGame = serviceProvider.GetService<IBattleShipGame>();
                var opponentBattleShipGame = serviceProvider.GetService<IBattleShipGame>();

                myBoard = MyBoard.GetMyBoard(myBattleShipGame.StrikeCoordinates, myBattleShipGame, opponentBattleShipGame, displayShips, boardSize);
                opponentBoard = OpponentBoard.GetOpponentBoard(myBattleShipGame, opponentBattleShipGame, boardSize);

                PrintBoard(); 

                //Uncommit to view the opponent ship coordinates. 
                DisplayCoordinates(myBattleShipGame, opponentBattleShipGame);
                GameProgress(displayShips, myBattleShipGame, opponentBattleShipGame);
            }
            else
            {
                WriteLine("Invalid input.");
            }
        }

        /// <summary>
        /// Display coordinates.
        /// </summary>
        /// <param name="myShips">Myships object.</param>
        /// <param name="opponentShips">Opponent object.</param>
        private static void DisplayCoordinates(IBattleShipGame myShips, IBattleShipGame opponentShips)
        {
            // Displaying the coordinates just to make it easier to review the code.
            WriteLine("--Displaying the coordinates just to make it easier to review the code.--");
            WriteLine(string.Join(",", myShips.Ship.Select(x => $"x{x.X}-y{x.Y}").ToList()));
            WriteLine("-------------------------------------------------------------------------");
            WriteLine(string.Join(",", opponentShips.Ship.Select(x => $"x{x.X}-y{x.Y}").ToList()));

        }

        /// <summary>
        /// Print the board for your ship and Opponent ship.
        /// </summary>
        private static void PrintBoard()
        {
            WriteLine();
            WriteLine("------------------------------ My Board ------------------------------");
            WriteLine();

            PrintBoardHeader();

            ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < boardSize + 1; x++)
            {
                for (int y = 0; y < boardSize + 1; y++)
                {
                    Write(myBoard[x, y]);
                    if (y == boardSize)
                    {
                        WriteLine("");
                    }
                }
            }

            WriteLine();
            WriteLine("---------------------- Opponent Board (Computer) --------------------");
            WriteLine();

            PrintBoardHeader();
            ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < boardSize + 1; x++)
            {
                for (int y = 0; y < boardSize + 1; y++)
                {
                    Write(opponentBoard[x, y]);
                    if (y == boardSize)
                    {
                        WriteLine("");
                    }
                }
            }
        }

        /// <summary>
        /// Game progress
        /// </summary>
        /// <param name="displayShips">Display your ships.</param>
        /// <param name="myShips">Object of BattleShipGame.</param>
        /// <param name="opponentShips">Object of BattleShipGame.</param>
        private static void GameProgress(bool displayShips, IBattleShipGame myShips, IBattleShipGame opponentShips)
        {
            Dictionary<char, int> xCoordinates = XCoordinate.Initialize(boardSize);

            int boardGame;
            // Default Board is 10X10 so there are 100 turns in total.
            for (boardGame = 1; boardGame < (boardSize * boardSize) + 1; boardGame++)
            {
                // Steps taken to complete.
                myShips.TurnCount++;

                ForegroundColor = ConsoleColor.White;
                WriteLine("Enter shooting coordinates for example (B8).");
                string input = ReadLine();

                Coordinate coordinate = new Coordinate();
                coordinate = Input.Analyze(input, xCoordinates);

                if (coordinate.X == -1 || coordinate.Y == -1)
                {
                    WriteLine("Invalid coordinates!");
                    boardGame--;
                    continue;
                }

                if (myShips.StrikeCoordinates.Any(s => s.X == coordinate.X && s.Y == coordinate.Y))
                {
                    WriteLine("The coordinate provided is already being used.");
                    boardGame--;
                    continue;
                }

                opponentShips.Shot(boardSize);

                if (myShips.StrikeCoordinates.FindIndex(p => p.X == coordinate.X && p.Y == coordinate.Y) == -1)
                {
                    myShips.StrikeCoordinates.Add(coordinate);
                }

                myShips.CheckShipStatus(opponentShips.StrikeCoordinates);

                opponentShips.CheckShipStatus(myShips.StrikeCoordinates);

                Clear();

                myBoard = MyBoard.GetMyBoard(myShips.StrikeCoordinates, myShips, opponentShips, displayShips, boardSize);
                opponentBoard = OpponentBoard.GetOpponentBoard(myShips, opponentShips, boardSize);

                PrintBoard();
                DisplayCoordinates(myShips, opponentShips);

                // Your result
                Results(myShips, true);

                // Opponent result
                Results(opponentShips, false);

                // Exit game is anyone wins
                if (opponentShips.IsBattleshipDestroyed || myShips.IsBattleshipDestroyed)
                { break; }
            }

            ForegroundColor = ConsoleColor.White;

            if (opponentShips.IsBattleshipDestroyed && !myShips.IsBattleshipDestroyed)
            {
                WriteLine("Congratualtions, you win.");
            }
            else if (!opponentShips.IsBattleshipDestroyed && myShips.IsBattleshipDestroyed)
            {
                WriteLine("Sorry, you lose.");
            }
            else
            {
                WriteLine("It's draw.");
            }

            WriteLine("Completed in: {0} steps.", boardGame);
            ReadLine();
        }

        /// <summary>
        /// Print board header
        /// </summary>
        private static void PrintBoardHeader()
        {
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("| |");
            for (int i = 1; i < boardSize + 1; i++)
            {
                Write("| " + i + " |");
            }
        }

        private static void Results(IBattleShipGame myShip, bool isMyShip)
        {
            if (myShip.CheckShip && myShip.IsBattleshipDestroyed)
            {
                ForegroundColor = ConsoleColor.DarkRed;
                WriteLine("{0} {1} has been destroyed!", isMyShip ? "Your" : "Opponent", nameof(myShip.Ship));
                myShip.CheckShip = false;
            }
        }
    }
}
