namespace AoC2024Unified.Solutions
{
    public class Day11Solution : IDaySolution
    {
        private const int DayNum = 11;
        private static readonly int[] NumOfBlinks = [25, 75];
        private const int Factor = 2024;

        private static List<string> GetStringRow(string input)
            => [.. input.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)];

        private static string[] SplitStone(string stone)
        {
            string lStone = stone[..(stone.Length / 2)];
            string rStone = stone[(stone.Length / 2)..];
            string rStoneFixed = rStone.TrimStart('0');

            return [lStone, rStoneFixed.Length > 0 ? rStoneFixed : "0"];
        }

        private static void Blink(List<string> stones)
        {
            int index = 0;

            do
            {
                string stone = stones[index];

                if (stone == "0")
                {
                    stones[index] = "1";
                }
                else if (stone.Length % 2 == 0)
                {
                    string[] newStones = SplitStone(stone);

                    stones[index] = newStones[0];
                    stones.Insert(index + 1, newStones[1]);

                    ++index;
                }
                else
                {
                    checked
                    {
                        ulong stoneNum = ulong.Parse(stone);
                        ulong newStoneNum = stoneNum * Factor;
                        stones[index] = $"{newStoneNum}";
                    }
                }

                ++index;
            } while (index < stones.Count);
        }

        public async Task Solve(bool isReal)
        {
            string input = await Common.ReadFile(isReal, DayNum);
            List<string> stones = GetStringRow(input);

            DateTime start = DateTime.Now;

            for (int i = 0; i < NumOfBlinks.Max(); ++i)
            {
                Blink(stones);

                if (NumOfBlinks.Contains(i + 1))
                {
                    Console.WriteLine($"There are now {stones.Count} stones.");
                }
            }

            DateTime end = DateTime.Now;
            TimeSpan diff = end - start;

            Console.WriteLine($"Took {diff.TotalSeconds} seconds.");
        }
    }
}