using AoC2024Unified.Extensions;

namespace AoC2024Unified.Solutions
{
    public class Day11Solution : IDaySolution
    {
        private const int DayNum = 11;
        private static readonly int[] NumOfBlinks = [25, 75];
        private const int Factor = 2024;
        private const int ThreadLoad = 100;

        private class SequenceSearchResult
        {
            public required List<ulong> Sequence { get; set; }

            public required int Index { get; set; }

            public int Remaining
            {
                get => Sequence.Count - Index - 1;
            }
        }

        private static ulong[] SplitStone(ulong stone, ulong digits)
        {
            ulong divisor = (ulong)Math.Pow(10, digits);
            ulong lStone = stone / divisor;
            ulong rStone = stone % divisor;

            return [lStone, rStone];
        }

        private static SequenceSearchResult? FindInSequence(
            List<List<ulong>> sequences, ulong num
        )
        {
            foreach (List<ulong> sequence in sequences)
            {
                for (int i = 0; i < sequence.Count - 1; ++i)
                {
                    if (sequence[i] == num)
                    {
                        return new SequenceSearchResult
                        {
                            Sequence = sequence,
                            Index = i
                        };
                    }
                }
            }

            return null;
        }

        private static void AddToSequences(
            List<List<ulong>> sequences, ulong prev, ulong next)
        {
            List<ulong>? endsWithPrev
                = sequences.FirstOrDefault((s) => s[^1] == prev);
            List<ulong>? startsWithNext
                = sequences.FirstOrDefault((s) => s[0] == next);

            if (endsWithPrev != null)
            {
                if (startsWithNext != null)
                {
                    endsWithPrev.AddRange(startsWithNext);
                    sequences.Remove(startsWithNext);
                }
                else
                {
                    endsWithPrev.Add(next);
                }
            }
            else
            {
                sequences.Add([prev, next]);
            }
        }

        private static List<ulong> ResultOf(
            ulong num, int loops, List<List<ulong>> sequences)
        {
            if (loops == 0)
            {
                return [num];
            }

            int remainingLoops = loops;
            SequenceSearchResult? prior = FindInSequence(sequences, num);

            while (prior != null)
            {
                int steps = Math.Min(remainingLoops, prior.Remaining);

                remainingLoops -= steps;
                ulong farthest = prior.Sequence[prior.Index + steps];

                if (remainingLoops == 0)
                {
                    return [farthest];
                }

                num = farthest;

                prior = FindInSequence(sequences, num);
            }

            checked
            {
                --remainingLoops;

                if (num == 0)
                {
                    AddToSequences(sequences, 0, 1);
                    return ResultOf(1, remainingLoops, sequences);
                }
                else
                {
                    ulong digits = num.GetNumberOfDigits();

                    if (digits % 2 == 0)
                    {
                        ulong[] newNums = SplitStone(num, digits / 2);

                        var result = new List<ulong>();
                        result.AddRange(
                            ResultOf(newNums[0], remainingLoops, sequences));
                        result.AddRange(
                            ResultOf(newNums[1], remainingLoops, sequences));
                        return result;
                    }
                    else
                    {
                        ulong newNum = num * Factor;
                        AddToSequences(sequences, num, newNum);
                        return ResultOf(newNum, remainingLoops, sequences);
                    }
                }
            }
        }

        public async Task Solve(bool isReal)
        {
            List<ulong> stones
                = (await Common.ReadNumberRows(isReal, DayNum))[0]
                .Select((n) => (ulong)n).ToList();

            var sequences = new List<List<ulong>>();
            var newList = new List<ulong>();

            foreach (ulong num in stones)
            {
                var result = ResultOf(num, 75, sequences);
                newList.AddRange(result);
            }

            Console.WriteLine(newList.Count);

            // DateTime start = DateTime.Now;

            // for (int i = 0; i < NumOfBlinks.Max(); ++i)
            // {
            //     stones = await Blink(stones);

            //     string notable = NumOfBlinks.Contains(i + 1) ? "***" : "";
            //     Console.WriteLine(
            //         $"{notable}{i + 1} blinks, {stones.Count} stones.");
            // }

            // DateTime end = DateTime.Now;
            // TimeSpan diff = end - start;

            // Console.WriteLine($"Took {diff.TotalSeconds} seconds.");
        }
    }
}