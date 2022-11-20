using FlareBattleShip.Models;
namespace FlareBattleShip.Classes
{
    /// <summary>
    /// Class Input.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Analyze input.
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="coordinates">X,Y coordinates.</param>
        /// <returns></returns>
        public static Coordinate Analyze(string input, Dictionary<char, int> coordinates)
        {
            Coordinate coordinate = new Coordinate();

            // To compare with the coordinates dictionary
            char[] splitInput = input.ToUpper().ToCharArray();

            // return the invalid input
            if (splitInput.Length < 2 || splitInput.Length > 4)
            {
                return coordinate;
            }

            // Get X coordinate.
            // Extract first character and get the value from coordinates dictionary
            if (coordinates.TryGetValue(splitInput[0], out int value))
            {
                coordinate.X = value;
            }
            else
            {
                return coordinate;
            }

            // Get Y coordinate.
            if (splitInput.Length == 3)
            {
                var value1 = splitInput[1] -'0';
                var value2 = splitInput[2] - '0';

                if (value1.GetType() == typeof(int) && value2.GetType() == typeof(int))
                {
                    coordinate.Y = int.Parse($"{value1}{value2}");
                    return coordinate;
                }
                else
                {
                    return coordinate;
                }
            }

            if (splitInput[1] - '0' > 9)
            {
                return coordinate;
            }
            else
            {
                coordinate.Y = splitInput[1] - '0';
            }

            return coordinate;
        }
    }
}
