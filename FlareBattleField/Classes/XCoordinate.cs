namespace FlareBattleField.Classes
{
    public static class XCoordinate
    {
        /// <summary>
        /// Initialize X-coordinates
        /// </summary>
        /// <param name="boardSize">Board size.</param>
        /// <returns>XCoordinates dictionary.</returns>
        public static Dictionary<char, int> Initialize(int boardSize)
        {
            char row = 'A';
            var xCorrdinates = new Dictionary<char, int>();
            for (int i = 0; i < boardSize; i++)
            {

                xCorrdinates[row] = i + 1;
                row++;
            }
            return xCorrdinates;
        }

    }
}
