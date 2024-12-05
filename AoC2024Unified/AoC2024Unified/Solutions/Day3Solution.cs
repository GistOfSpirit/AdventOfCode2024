using System.Text.RegularExpressions;
using AoC2024Unified.ThreeLang;

namespace AoC2024Unified.Solutions
{
    public class Day3Solution : IDaySolution
    {
        const int DayNum = 3;

        private static int SolveA(string inputString)
        {
            string pattern = @"mul\((?<num1>[0-9]+),(?<num2>[0-9]+)\)";

            MatchCollection matches = Regex.Matches(inputString, pattern,
                RegexOptions.None, TimeSpan.FromSeconds(3))
                ?? throw new InvalidOperationException("Null object matches");

            int total = 0;

            foreach (Match match in matches)
            {
                int num1 = Convert.ToInt32(match.Groups["num1"].Value);
                int num2 = Convert.ToInt32(match.Groups["num2"].Value);

                total += num1 * num2;
            }

            return total;
        }

        private static async Task<int> SolveA(bool isReal, string sub = "")
        {
            string inputString = await Common.ReadFile(isReal, DayNum, sub);

            return SolveA(inputString);
        }

        private static async Task<int> SolveB(bool isReal, string sub = "")
        {
            string inputString = await Common.ReadFile(isReal, DayNum, sub);

            var parser = new ThreeLangParser(inputString);

            return parser.ParseCode();
        }

        public async Task Solve(bool isReal)
        {
            int totalA, totalB;

            if (isReal)
            {
                totalA = await SolveA(isReal);
                totalB = await SolveB(isReal);
            }
            else
            {
                totalA = await SolveA(isReal, "a");
                totalB = await SolveB(isReal, "b");
            }

            Console.WriteLine($"The totals are A:{totalA}, B:{totalB}");
        }
    }
}