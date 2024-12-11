using AoC2024Unified.Extensions;

namespace AoC2024Unified.Solutions
{
    public class Day11Solution : IDaySolution
    {
        private const int DayNum = 11;
        private static readonly int[] NumOfBlinks = [25, 75];
        private const int Factor = 2024;

        private static ulong[] SplitStone(ulong stone, int digits)
        {
            ulong divisor = (ulong)Math.Pow(10, digits / 2);
            ulong lStone = stone / divisor;
            ulong rStone = stone % divisor;

            return [lStone, rStone];
        }

        private static List<ulong> Blink(ulong stone)
        {
            if (stone == 0)
            {
                return [1];
            }
            else
            {
                int digits = stone.GetNumberOfDigits();

                if (digits % 2 == 0)
                {
                    ulong[] newStones = SplitStone(stone, digits);

                    return [.. newStones];
                }
                else
                {
                    checked
                    {
                        ulong newStoneNum = stone * Factor;
                        return [newStoneNum];
                    }
                }
            }
        }

        private static ulong CalcResult(List<ulong> stones, int blinks,
            Dictionary<(ulong stone, int blinks), ulong> priorCalcs)
        {
            if (blinks == 0)
            {
                return (ulong)stones.Count;
            }

            checked
            {
                ulong total = 0;

                foreach (ulong stone in stones)
                {
                    if (priorCalcs.ContainsKey((stone, blinks)))
                    {
                        total += priorCalcs[(stone, blinks)];
                        continue;
                    }

                    List<ulong> newStones = Blink(stone);
                    ulong result = CalcResult(newStones, blinks - 1, priorCalcs);
                    priorCalcs.Add((stone, blinks), result);
                    total += result;
                }

                return total;
            }
        }

        public async Task Solve(bool isReal)
        {
            List<ulong> stones =
                (await Common.ReadNumberRows(isReal, DayNum))[0]
                .Select((n) => (ulong)n)
                .ToList();

            var priorCalcs = new Dictionary<(ulong stone, int blinks), ulong>();

            DateTime start = DateTime.Now;

            foreach (int blinks in NumOfBlinks)
            {
                ulong result = CalcResult(stones, blinks, priorCalcs);

                Console.WriteLine($"{blinks} blinks: {result}");
            }

            DateTime end = DateTime.Now;
            TimeSpan diff = end - start;

            Console.WriteLine($"Took {diff.TotalSeconds} seconds.");
        }
    }
}