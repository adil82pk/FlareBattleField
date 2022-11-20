using FlareBattleShip.Interfaces;
namespace FlareBattleShip.Models
{
    /// <summary>
    /// Class MyBoard.
    /// </summary>
    public static class MyBoard
    {
        /// <summary>
        /// Get my board coordinates to draw.
        /// </summary>
        /// <param name="coordinates">Collection of Coordinates.</param>
        /// <param name="myBattleShip">Object of BattleShipGame.</param>
        /// <param name="opponentBattleShip">Object of BattleShipGame.</param>
        /// <param name="displayOpponentShips">True to display opponent ship else false.</param>
        /// <param name="boardSize">Board size.</param>
        /// <returns>Coordinates to draw the positions of the ships on the board.</returns>
        public static string[,] GetMyBoard(List<Coordinate> coordinates, IBattleShipGame myBattleShip, IBattleShipGame opponentBattleShip,
            bool displayOpponentShips, int boardSize)
        {
            string[,] myBoard = new string[boardSize + 1, boardSize + 1];
            if (!displayOpponentShips)
            {
                // For the case of hit display the ship.
                displayOpponentShips = myBattleShip.IsBattleshipDestroyed;
            }

            List<Coordinate> sortedHitCoordinates = coordinates.OrderBy(o => o.X).ThenBy(n => n.Y).ToList();
            List<Coordinate> sortedShipsCoordinates = opponentBattleShip.Ship.OrderBy(o => o.X).ThenBy(n => n.Y).ToList();

            // Filter out ships already being hit.      
            sortedShipsCoordinates = sortedShipsCoordinates.Where(s => !sortedHitCoordinates.Exists(ShipPos => ShipPos.X == s.X && ShipPos.Y == s.Y)).ToList();

            int hitCounter = 0;
            int opponentShipCounter = 0;

            char row = 'A';
            try
            {
                // Start from 1 as 0 is the position to draw the horizontal headers
                for (int x = 1; x < boardSize + 1; x++)
                {
                    // Start from 1 as 0 is the position to draw the vertical headers
                    for (int y = 1; y < boardSize + 1; y++)
                    {

                        bool proceed = true;

                        if (y == 1)
                        {
                            // Add characters i.e (A, B, C ...) on the Y axis on the 0th postion 
                            myBoard[x, y - 1] = "|" + row + "|";
                            
                            // increment character 
                            row++;
                        }

                        if (sortedHitCoordinates.Count != 0 && sortedHitCoordinates[hitCounter].X == x && sortedHitCoordinates[hitCounter].Y == y)
                        {
                            if (sortedHitCoordinates.Count - 1 > hitCounter)
                            {
                                hitCounter++;
                            }

                            if (opponentBattleShip.Ship.Exists(ShipPos => ShipPos.X == x && ShipPos.Y == y))
                            {
                                myBoard[x, y] = "| H |";
                                proceed = false;
                            }
                            else
                            {
                                myBoard[x, y] = "| X |";
                                proceed = false;
                            }
                        }

                        if (proceed && displayOpponentShips && sortedShipsCoordinates.Count != 0 &&
                            sortedShipsCoordinates[opponentShipCounter].X == x && sortedShipsCoordinates[opponentShipCounter].Y == y)
                        {

                            if (sortedShipsCoordinates.Count - 1 > opponentShipCounter)
                                opponentShipCounter++;

                            myBoard[x, y] = "| U |";
                            proceed = false;
                        }

                        if (proceed)
                        {
                            myBoard[x, y] = "| - |";
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return myBoard;
        }
    }
}
