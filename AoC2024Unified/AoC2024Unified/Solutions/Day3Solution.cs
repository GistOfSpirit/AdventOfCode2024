using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        private static async Task<int> SolveA(string dayNum, bool isReal)
        {
            string inputString = await Common.ReadFile(dayNum, isReal);

            return SolveA(inputString);
        }

        private static void AddIndexes(List<(int, bool)> list,
            string input, string instr, bool value)
        {
            MatchCollection matches = Regex.Matches(input, Regex.Escape(instr),
                RegexOptions.None, TimeSpan.FromSeconds(3))
                ?? throw new InvalidOperationException("Null object matches");

            foreach (Match match in matches)
            {
                list.Add((match.Index, value));
            }
        }

        private static async Task<int> SolveB(string dayNum, bool isReal)
        {
            const string instrOn = "do()";
            const string instrOff = "don't()";

            string inputString = await Common.ReadFile(dayNum, isReal);

            var instrPositions = new List<(int, bool)>();

            AddIndexes(instrPositions, inputString, instrOn, true);
            AddIndexes(instrPositions, inputString, instrOff, false);

            instrPositions.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            var newInputSb = new StringBuilder();

            newInputSb.Append(inputString[..instrPositions[0].Item1]);

            for (int i = 0; i < instrPositions.Count; ++i)
            {
                (int, bool) item = instrPositions[i];

                if (item.Item2)
                {
                    int nextIndex = instrPositions.Count <= i + 1
                        ? inputString.Length - 1
                        : instrPositions[i + 1].Item1;

                    newInputSb.Append(
                        inputString[item.Item1..nextIndex]);
                }
            }

            // There is a bug here: We've effectively removed the "don't()"
            // sections. If the input text had something like
            // "mudon't()fdsfdfdsfdsfdsffdo()l(2,3)"
            // this would turn into mul(2,3) and do an extra multiplication.
            // I might fix this later.
            return SolveA(newInputSb.ToString());
        }

        public async Task Solve(bool isReal)
        {
            int totalA, totalB;

            if (isReal)
            {
                string dayNum = $"{DayNum}";
                totalA = await SolveA(dayNum, isReal);
                totalB = await SolveB(dayNum, isReal);
            }
            else
            {
                totalA = await SolveA($"{DayNum}a", isReal);
                totalB = await SolveB($"{DayNum}b", isReal);
            }

            Console.WriteLine($"The totals are A:{totalA}, B:{totalB}.");
        }
    }
}