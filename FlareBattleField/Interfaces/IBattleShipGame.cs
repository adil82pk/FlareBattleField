using FlareBattleShip.Models;
using FlareBattleShip.Classes;
namespace FlareBattleShip.Interfaces
{
    /// <summary>
    /// Interface IBattleShipGame.
    /// </summary>
    public interface IBattleShipGame
    {
        bool CheckShip { get; set; }
        bool IsBattleshipDestroyed { get; set; }
        List<Coordinate> Ship { get; set; }
        List<Coordinate> StrikeCoordinates { get; set; }
        int TurnCount { get; set; }

        /// <summary>
        /// Check ship status for both Your and opponent ships.
        /// If count is ZERO all ships are destroyed.
        /// </summary>
        /// <param name="HitCoordinates">Collection of Coordinates</param>
        /// <returns>Object of BattleShipGame.</returns>
        BattleShipGame CheckShipStatus(List<Coordinate> HitCoordinates);

        /// <summary>
        /// Innitalize Ships Position horizontally or vertically.
        /// </summary>
        /// <param name="ships">Number of ships.</param>
        /// <param name="boardSize">Board size.</param>
        /// <returns>Collection of Coordinates.</returns>
        List<Coordinate> InnitalizePosition(int ships, int boardSize);

        /// <summary>
        /// Recurrsively generate new coordinates for the opponent i.e Computer.
        /// </summary>
        /// <param name="boardSize">Board size.</param>
        /// <returns>Object of BattleShipGame with strike positions.</returns>
        BattleShipGame Shot(int boardSize);
    }
}