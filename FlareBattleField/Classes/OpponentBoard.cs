using FlareBattleShip.Interfaces;
namespace FlareBattleShip.Models
{
    /// <summary>
    /// Class OpponentBoard.
    /// </summary>
    public static class OpponentBoard
    {
        /// <summary>
        /// Get opponent board.
        /// </summary>
        /// <param name="myShip">Object of BattleShipGame.</param>
        /// <param name="opponentShip">Object of BattleShipGame.</param>
        /// <param name="boardSize">Board size.</param>
        /// <returns>Coordinates to draw the positions of the ships on the board.</returns>
        public static string[,] GetOpponentBoard(IBattleShipGame myShip, IBattleShipGame opponentShip, int boardSize)
        {
            string[,] opponentBoard = new string[boardSize + 1, boardSize + 1];
            char row = 'A';

            int myshipCounter = 0;
            int opponentHitCounter = 0;

            List<Coordinate> sortedOpponentCoordinates = opponentShip.StrikeCoordinates.OrderBy(o => o.X).ThenBy(n => n.Y).ToList();
            List<Coordinate> sortedMyShipCoordinates = myShip.Ship.OrderBy(o => o.X).ThenBy(n => n.Y).ToList();

            try
            {
                for (int x = 1; x < boardSize + 1; x++)
                {
                    for (int y = 1; y < boardSize + 1; y++)
                    {
                        bool proccess = true;

                        if (sortedOpponentCoordinates.Count != 0 && sortedOpponentCoordinates[opponentHitCounter].X == x
                            && sortedOpponentCoordinates[opponentHitCounter].Y == y)
                        {
                            if (sortedOpponentCoordinates.Count - 1 > opponentHitCounter)
                            {
                                opponentHitCounter++;
                            }

                            if (myShip.Ship.Exists(s => s.X == x && s.Y == y))
                            {
                                opponentBoard[x, y] = "| H |";
                                proccess = false;
                            }
                            else
                            {
                                opponentBoard[x, y] = "| X |";
                                proccess = false;
                            }
                        }

                        if (proccess && sortedMyShipCoordinates.Count != 0 && sortedMyShipCoordinates[myshipCounter].X == x
                            && sortedMyShipCoordinates[myshipCounter].Y == y)
                        {
                            if (sortedMyShipCoordinates.Count - 1 > myshipCounter)
                            {
                                myshipCounter++;
                            }

                            opponentBoard[x, y] = "| U |";
                            proccess = false;
                        }

                        if (y == 1)
                        {
                            opponentBoard[x, y - 1] = "|" + row + "|";
                            row++;
                        }

                        if (proccess)
                        {
                            opponentBoard[x, y] = "| - |";
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return opponentBoard;
        }

    }
}
