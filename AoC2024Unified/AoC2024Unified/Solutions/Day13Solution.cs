using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC2024Unified.Solutions
{
    public class Day13Solution : IDaySolution
    {
        private const int DayNum = 13;
        private const int MaxPresses = 100;
        private const int ACost = 3;
        private const int BCost = 1;

        private class Machine
        {
            public Size MoveA { get; set; }
            public Size MoveB { get; set; }
            public Point PrizeLocation { get; set; }
        }

        private struct PressOption
        {
            public int ATimes { get; set; }
            public int BTimes { get; set; }
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

        private static IEnumerable<(int times, int rem)> CalcMultiples(
            int factor, int target)
        {
            for (int i = 1; i <= MaxPresses; ++i)
            {
                int multiple = i * factor;

                if (multiple > target)
                {
                    yield break;
                }

                yield return (i, target - multiple);
            }
        }

        private static List<PressOption> CalcOptions(
            Machine mach)
        {
            var list = new List<PressOption>();

            foreach (var (aTimes, aRem)
                in CalcMultiples(mach.MoveA.Width, mach.PrizeLocation.X))
            {
                if (aRem % mach.MoveB.Width == 0)
                {
                    // Reached X, but have we reached Y?
                    int bTimes = aRem / mach.MoveB.Width;

                    if (
                        (aTimes * mach.MoveA.Height)
                        + (bTimes * mach.MoveB.Height)
                        == mach.PrizeLocation.Y
                    )
                    {
                        // This is an option
                        list.Add(new PressOption
                        {
                            ATimes = aTimes,
                            BTimes = bTimes
                        });
                    }
                }
            }

            return list;
        }

        private static int CalcCost(PressOption option)
            => (option.ATimes * ACost) + (option.BTimes * BCost);

        private static int CalcCheapestOption(Machine mach)
        {
            var options = CalcOptions(mach);

            if (options.Count == 0)
            {
                // Impossible
                return -1;
            }

            int cheapest = CalcCost(options[0]);

            foreach (var option in options[1..])
            {
                cheapest = Math.Min(cheapest, CalcCost(option));
            }

            return cheapest;
        }

        public async Task Solve(bool isReal)
        {
            string input = await Common.ReadFile(isReal, DayNum);
            var machines = ParseInput(input);

            int total = 0;

            foreach (Machine mach in machines)
            {
                int cheapest = CalcCheapestOption(mach);

                if (cheapest >= 0)
                {
                    total += cheapest;
                }
            }

            Console.WriteLine($"Spend {total} tokens");
        }
    }
}