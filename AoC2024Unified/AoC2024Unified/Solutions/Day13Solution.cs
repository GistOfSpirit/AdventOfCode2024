using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC2024Unified.Solutions
{
    public class Day13Solution : IDaySolution
    {
        private const int DayNum = 13;
        private const int ACost = 3;
        private const int BCost = 1;

        private class Machine
        {
            public Size MoveA { get; set; }
            public Size MoveB { get; set; }
            public Point PrizeLocation { get; set; }
        }

        private static (int x, int y) ParseMatch(Match match)
            => (int.Parse(match.Groups["X"].Value),
                int.Parse(match.Groups["Y"].Value));

        private static List<Machine> ParseInput(string input)
        {
            string patternA = @"Button A: X\+(?<X>[0-9]+), Y\+(?<Y>[0-9]+)";
            string patternB = @"Button B: X\+(?<X>[0-9]+), Y\+(?<Y>[0-9]+)";
            string patternP = @"Prize: X=(?<X>[0-9]+), Y=(?<Y>[0-9]+)";

            string[] inputParts = input.Split(
                Environment.NewLine + Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

            var list = new List<Machine>();

            foreach (string strMach in inputParts)
            {
                var matchA = Regex.Match(strMach, patternA);
                var coordsA = ParseMatch(matchA);

                var matchB = Regex.Match(strMach, patternB);
                var coordsB = ParseMatch(matchB);

                var matchP = Regex.Match(strMach, patternP);
                var coordsP = ParseMatch(matchP);

                list.Add(new Machine
                {
                    MoveA = new Size(coordsA.x, coordsA.y),
                    MoveB = new Size(coordsB.x, coordsB.y),
                    PrizeLocation = new Point(coordsP.x, coordsP.y)
                });
            }

            return list;
        }

        private static int CalcCost(int timesA, int timesB)
            => (timesA * ACost) + (timesB * BCost);

        private static int CalcTimes(Machine mach)
        {
            int div = mach.MoveA.Width * mach.MoveB.Height
                    - mach.MoveA.Height * mach.MoveB.Width;
            int timesA = (
                    mach.PrizeLocation.X * mach.MoveB.Height
                    - mach.PrizeLocation.Y * mach.MoveB.Width
                ) / div;
            int timesB = (
                    mach.PrizeLocation.Y * mach.MoveA.Width
                    - mach.PrizeLocation.X * mach.MoveA.Height
                ) / div;

            if (
                (timesA * mach.MoveA.Width + timesB * mach.MoveB.Width
                    == mach.PrizeLocation.X)
                && (timesA * mach.MoveA.Height + timesB * mach.MoveB.Height
                    == mach.PrizeLocation.Y)
            )
            {
                return CalcCost(timesA, timesB);
            }

            return 0;
        }

        public async Task Solve(bool isReal)
        {
            string input = await Common.ReadFile(isReal, DayNum);
            var machines = ParseInput(input);

            int total = 0;

            foreach (Machine mach in machines)
            {
                int cheapest = CalcTimes(mach);

                total += cheapest;
            }

            Console.WriteLine($"Spend {total} tokens");
        }
    }
}