namespace AoC2024Unified.Solutions
{
    public class Day4BSolution : IDaySolution
    {
        const int DayNum = 4;
        const string Pattern = "MAS";

        private static bool CheckForInstanceSE(string[] matrix,
            int row, int col)
        {
            int halfPatternLength = Pattern.Length / 2;

            bool isForwards = true;
            bool isBackwards = true;

            for (
                int i = -halfPatternLength;
                i <= halfPatternLength; ++i)
            {
                if (
                    isForwards
                    && (matrix[row + i][col + i]
                        != Pattern[i + halfPatternLength])
                )
                {
                    isForwards = false;
                }

                if (
                    isBackwards
                    && (matrix[row + i][col + i]
                        != Pattern[^(i + halfPatternLength + 1)])
                )
                {
                    isBackwards = false;
                }
            }

            return isForwards || isBackwards;
        }

        private static bool CheckForInstanceSW(string[] matrix,
            int row, int col)
        {
            int halfPatternLength = Pattern.Length / 2;

            bool isForwards = true;
            bool isBackwards = true;

            for (
                int i = -halfPatternLength;
                i <= halfPatternLength; ++i)
            {
                if (
                    isForwards
                    && (matrix[row + i][col - i]
                        != Pattern[i + halfPatternLength])
                )
                {
                    isForwards = false;
                }

                if (
                    isBackwards
                    && (matrix[row + i][col - i]
                        != Pattern[^(i + halfPatternLength + 1)])
                )
                {
                    isBackwards = false;
                }
            }

            return isForwards || isBackwards;
        }

        public async Task Solve(bool isReal)
        {
            string[] matrix = await Common.ReadFileAsMatrix(
                isReal, DayNum);

            int halfPatternLength = Pattern.Length / 2;

            int total = 0;

            for (int row = halfPatternLength;
                row <= matrix.Length - halfPatternLength - 1; ++row)
            {
                for (int col = halfPatternLength;
                    col <= matrix[row].Length - halfPatternLength - 1; ++col)
                {
                    if (CheckForInstanceSE(matrix, row, col)
                        && CheckForInstanceSW(matrix, row, col))
                    {
                        ++total;
                    }
                }
            }

            Console.WriteLine(
                $"There are {total} instances of \"X-{Pattern}\".");
        }
    }
}