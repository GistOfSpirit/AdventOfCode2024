
namespace AoC2024Unified.Solutions
{
    public class Day7Solution : IDaySolution
    {
        private const int DayNum = 7;

        private class CalcRow
        {
            public required ulong Result { init; get; }

            public required List<ulong> Numbers { init; get; }
        }

        private static List<CalcRow> ParseFile(string input)
        {
            string[] rows = input.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);
            var calcRows = new List<CalcRow>();

            foreach (string row in rows)
            {
                string[] numbers = row.Split(' ',
                    StringSplitOptions.RemoveEmptyEntries);

                calcRows.Add(new CalcRow
                {
                    Result = ulong.Parse(numbers[0][..^1]),
                    Numbers = [.. numbers[1..].Select(ulong.Parse)]
                });
            }

            return calcRows;
        }

        private static IEnumerable<ulong> GetPossLineResults(
            List<ulong> numbers, bool useConcat, int reverseIndex = 1)
        {
            ulong thisNum = numbers[^reverseIndex];

            if (reverseIndex == numbers.Count)
            {
                yield return thisNum;
            }
            else
            {
                foreach (ulong restNum in GetPossLineResults(
                    numbers, useConcat, reverseIndex + 1))
                {
                    yield return thisNum + restNum;
                    yield return thisNum * restNum;

                    if (useConcat)
                    {
                        string combo = $"{restNum}{thisNum}";
                        ulong comboNum = ulong.Parse(combo);

                        yield return comboNum;
                    }
                }
            }
        }

        private static ulong CalculateTotal(
            List<CalcRow> calcRows, bool useConcat)
        {
            ulong total = 0;

            foreach (CalcRow row in calcRows)
            {
                foreach (ulong possCalc in GetPossLineResults(
                    row.Numbers, useConcat))
                {
                    if (possCalc == row.Result)
                    {
                        total += row.Result;
                        break;
                    }
                }
            }

            return total;
        }

        public async Task Solve(bool isReal)
        {
            string input = await Common.ReadFile(isReal, DayNum);
            List<CalcRow> calcRows = ParseFile(input);

            ulong total = CalculateTotal(calcRows, false);
            ulong concatTotal = CalculateTotal(calcRows, true);

            Console.WriteLine($"The total is {total}.");
            Console.WriteLine(
                $"The total with concatenation is {concatTotal}.");
        }
    }
}