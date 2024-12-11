using AoC2024Unified.Extensions;

namespace AoC2024Unified.Solutions
{
    public class Day11Solution : IDaySolution
    {
        private const int DayNum = 11;
        private static readonly int[] NumOfBlinks = [25, 75];
        private const int Factor = 2024;
        private const int ThreadLoad = 100;

        private static ulong[] SplitStone(ulong stone, ulong digits)
        {
            ulong divisor = (ulong)Math.Pow(10, digits);
            ulong lStone = stone / divisor;
            ulong rStone = stone % divisor;

            return [lStone, rStone];
        }

        private static IEnumerable<List<ulong>> SplitList(List<ulong> stones)
        {
            int start = 0;

            while (start < stones.Count)
            {
                List<ulong> subStones = stones
                    .Skip(start).Take(ThreadLoad).ToList();

                yield return subStones;
                start += ThreadLoad;
            }
        }

        private static async Task<List<ulong>> Blink(List<ulong> stones)
        {
            if (stones.Count > ThreadLoad)
            {
                var taskList = new List<Task<List<ulong>>>();

                foreach (List<ulong> partList in SplitList(stones))
                {
                    taskList.Add(Blink(partList));
                }

                var newList = new List<ulong>();

                for (int i = 0; i < taskList.Count; ++i)
                {
                    newList.AddRange(await taskList[i]);
                }

                return newList;
            }

            checked
            {
                int index = 0;
                var newList = new List<ulong>(stones);

                do
                {
                    ulong stone = newList[index];

                    if (stone == 0)
                    {
                        newList[index] = 1;
                    }
                    else
                    {
                        ulong digits = stone.GetNumberOfDigits();

                        if (digits % 2 == 0)
                        {
                            ulong[] newStones = SplitStone(stone, digits / 2);

                            newList[index] = newStones[0];
                            newList.Insert(++index, newStones[1]);
                        }
                        else
                        {
                            ulong newStoneNum = stone * Factor;
                            newList[index] = newStoneNum;
                        }
                    }

                    ++index;
                } while (index < newList.Count);

                return newList;
            }
        }

        public async Task Solve(bool isReal)
        {
            List<ulong> stones
                = (await Common.ReadNumberRows(isReal, DayNum))[0]
                .Select((n) => (ulong)n).ToList();

            DateTime start = DateTime.Now;

            for (int i = 0; i < NumOfBlinks.Max(); ++i)
            {
                stones = await Blink(stones);

                string notable = NumOfBlinks.Contains(i + 1) ? "***" : "";
                Console.WriteLine(
                    $"{notable}{i + 1} blinks, {stones.Count} stones.");
            }

            DateTime end = DateTime.Now;
            TimeSpan diff = end - start;

            Console.WriteLine($"Took {diff.TotalSeconds} seconds.");
        }
    }
}