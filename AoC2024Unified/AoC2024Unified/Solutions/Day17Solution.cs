
using System.Text.RegularExpressions;
using AoC2024Unified.Solutions.Day17;

namespace AoC2024Unified.Solutions
{
    public class Day17Solution : IDaySolution
    {
        private const int DayNum = 17;

        private static Machine ParseInput(string input)
        {
            string patternR = @"Register A: (?<regA>[0-9]+).*"
                + @"Register B: (?<regB>[0-9]+).*"
                + @"Register C: (?<regC>[0-9]+).*"
                + @"Program: (?<prog>(?:[0-9],?)+)";

            var match = Regex.Match(input, patternR, RegexOptions.Singleline);

            int a = int.Parse(match.Groups["regA"].Value);
            int b = int.Parse(match.Groups["regB"].Value);
            int c = int.Parse(match.Groups["regC"].Value);

            List<int> p = match.Groups["prog"].Value.Split(',')
                .Select(int.Parse)
                .ToList();

            return new Machine(a, b, c, p);
        }

        public async Task Solve(bool isReal)
        {
            string input = await Common.ReadFile(isReal, DayNum);
            Machine mach = ParseInput(input);

            List<int> output = mach.Run();
            string result = string.Join(',', output);

            Console.WriteLine($"Machine output: {result}");
        }
    }
}