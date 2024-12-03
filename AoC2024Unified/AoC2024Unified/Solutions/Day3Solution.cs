using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2024Unified.Solutions
{
    public class Day3Solution : IDaySolution
    {
        const int DayNum = 3;

        private static async Task SolveA(string dayNum, bool isReal)
        {
            string inputString = await Common.ReadFile(dayNum, isReal);

            string pattern = @"mul\((?<num1>[0-9]+),(?<num2>[0-9]+)\)";

            var matches = Regex.Matches(inputString, pattern,
                RegexOptions.None, TimeSpan.FromSeconds(3))
                ?? throw new InvalidOperationException("Null object matches");

            int total = 0;

            foreach (Match match in matches)
            {
                int num1 = Convert.ToInt32(match.Groups["num1"].Value);
                int num2 = Convert.ToInt32(match.Groups["num2"].Value);

                total += num1 * num2;
            }

            Console.WriteLine($"The total is {total}.");
        }

        private static async Task SolveB(string dayNum, bool isReal)
        {
            await Task.Delay(0);
        }

        public async Task Solve(bool isReal)
        {
            if (isReal)
            {
                string dayNum = $"{DayNum}";
                await SolveA(dayNum, isReal);
                await SolveB(dayNum, isReal);
            }
            else
            {
                await SolveA($"{DayNum}a", isReal);
                await SolveB($"{DayNum}b", isReal);
            }
        }
    }
}