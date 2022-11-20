using FlareBattleShip.Models;
using FlareBattleShip.Interfaces;
namespace FlareBattleShip.Classes
{
    /// <summary>
    /// Class BattleShipGame.
    /// </summary>
    public class BattleShipGame : IBattleShipGame
    {
        /// <summary>
        /// Constructor of class BattleShipGame.
        /// </summary>
        /// <param name="battleships"></param>
        public BattleShipGame(int battleships, int boardSize)
        {
            Ship = InnitalizePosition(battleships, boardSize);
        }

        public int TurnCount { get; set; } = 0;
        Random random = new Random();
        public List<Coordinate> Ship { get; set; }
        public List<Coordinate> StrikeCoordinates { get; set; } = new List<Coordinate>();
        public bool IsBattleshipDestroyed { get; set; } = false;
        public bool CheckShip { get; set; } = true;

        /// <summary>
        /// Recurrsively generate new coordinates for the opponent i.e Computer.
        /// </summary>
        /// <param name="boardSize">Board size.</param>
        /// <returns>Object of BattleShipGame with strike positions.</returns>
        public BattleShipGame Shot(int boardSize)
        {
            Coordinate opponentShotCorrdinates = new Coordinate();

            opponentShotCorrdinates.X = random.Next(1, boardSize + 1);
            opponentShotCorrdinates.Y = random.Next(1, boardSize + 1);
            if (StrikeCoordinates.Any(s => s.X == opponentShotCorrdinates.X && s.Y == opponentShotCorrdinates.Y))
            {
                Shot(boardSize);
            }

            StrikeCoordinates.Add(opponentShotCorrdinates);
            return this;
        }

        /// <summary>
        /// Check ship status for both Your and opponent ships.
        /// If count is ZERO all ships are destroyed.
        /// </summary>
        /// <param name="HitCoordinates">Collection of Coordinates</param>
        /// <returns>Object of BattleShipGame.</returns>
        public BattleShipGame CheckShipStatus(List<Coordinate> HitCoordinates)
        {
            IsBattleshipDestroyed = Ship.Where(b => !HitCoordinates.Any(h => b.X == h.X && b.Y == h.Y)).ToList().Count == 0;
            return this;
        }

        /// <summary>
        /// Innitalize Ships Position horizontally or vertically.
        /// </summary>
        /// <param name="ships">Number of ships.</param>
        /// <param name="boardSize">Board size.</param>
        /// <returns>Collection of Coordinates.</returns>
        public List<Coordinate> InnitalizePosition(int ships, int boardSize)
        {
            return GenerateRandomPositions(ships, boardSize);
        }

        /// <summary>
        /// Generate random positions.
        /// </summary>
        /// <param name="ships">Number of ships.</param>
        /// <param name="boardSize">Board size.</param>
        /// <returns>Collection of Coordinates.</returns>
        private List<Coordinate> GenerateRandomPositions(int ships, int boardSize)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            int row = random.Next(1, boardSize + 1);
            int column = random.Next(1, boardSize + 1);

            // True horizontal else vertical
            if (random.Next(2) == 1)
            {
                // From left to right
                if (row - ships > 0)
                {
                    for (int i = 0; i < ships; i++)
                    {
                        var coordinate = new Coordinate();
                        coordinate.X = row - i;
                        coordinate.Y = column;
                        coordinates.Add(coordinate);
                    }
                }
                else // Row
                {
                    for (int i = 0; i < ships; i++)
                    {
                        Coordinate coordinate = new Coordinate();
                        coordinate.X = row + i;
                        coordinate.Y = column;
                        coordinates.Add(coordinate);
                    }
                }
            }
            else
            {
                // From top to bottom
                if (column - ships > 0)
                {
                    for (int i = 0; i < ships; i++)
                    {
                        Coordinate coordinate = new Coordinate();
                        coordinate.X = row;
                        coordinate.Y = column - i;
                        coordinates.Add(coordinate);
                    }
                }
                else // Row 
                {
                    for (int i = 0; i < ships; i++)
                    {
                        Coordinate coordinate = new Coordinate();
                        coordinate.X = row;
                        coordinate.Y = column + i;
                        coordinates.Add(coordinate);
                    }
                }
            }

            return coordinates;
        }
    }
}
