using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day4ASolution : IDaySolution
    {
        const int DayNum = 4;
        const string Pattern = "XMAS";

        private static List<Direction> GetPossibleDirections(string[] matrix,
            int row, int col)
        {
            List<Direction> dirList = Directions.GetSoutheastHalf();

            // Too far north
            if (row < Pattern.Length - 1)
            {
                dirList.Remove(Direction.Northeast);
            }

            // Too far south
            if (row > matrix.Length - Pattern.Length)
            {
                dirList.Remove(Direction.South);
                dirList.Remove(Direction.Southeast);
            }

            // Too far east
            if (col > matrix[row].Length - Pattern.Length)
            {
                dirList.Remove(Direction.East);
                dirList.Remove(Direction.Northeast);
                dirList.Remove(Direction.Southeast);
            }

            return dirList;
        }

        private static void AdvancePosition(Direction direction,
            ref int row, ref int col)
        {
            Point nextPoint = direction.GetNextPoint(new Point(col, row));
            row = nextPoint.Y;
            col = nextPoint.X;
        }

        private static bool CheckForInstanceForwards(string[] matrix,
            int row, int col, Direction direction)
        {
            for (int i = 0; i < Pattern.Length; ++i)
            {
                if (matrix[row][col] != Pattern[i])
                {
                    return false;
                }

                AdvancePosition(direction, ref row, ref col);
            }

            return true;
        }

        private static bool CheckForInstanceBackwards(string[] matrix,
            int row, int col, Direction direction)
        {
            for (int i = 0; i < Pattern.Length; ++i)
            {
                if (matrix[row][col] != Pattern[^(i + 1)])
                {
                    return false;
                }

                AdvancePosition(direction, ref row, ref col);
            }

            return true;
        }

        private static int GetInstancesAtPosition(string[] matrix,
            int row, int col)
        {
            var possDirections = GetPossibleDirections(matrix, row, col);

            return possDirections
                .Count((dir) =>
                    CheckForInstanceForwards(matrix, row, col, dir)
                    || CheckForInstanceBackwards(matrix, row, col, dir));
        }

        public async Task Solve(bool isReal)
        {
            string[] matrix = await Common.ReadFileAsMatrix(
                isReal, DayNum);

            int total = 0;

            for (int row = 0; row < matrix.Length; ++row)
            {
                for (int col = 0; col < matrix[row].Length; ++col)
                {
                    total += GetInstancesAtPosition(matrix, row, col);
                }
            }

            Console.WriteLine($"There are {total} instances of \"{Pattern}\".");
        }
    }
}