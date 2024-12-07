
namespace AoC2024Unified.Solutions
{
    public class Day7Solution : IDaySolution
    {
        private const int DayNum = 7;

        private class CalcRow
        {
            public required UInt128 Result { init; get; }

            public required List<UInt128> Numbers { init; get; }
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
                    Result = UInt128.Parse(numbers[0][..^1]),
                    Numbers = [.. numbers[1..].Select(UInt128.Parse)]
                });
            }

            return calcRows;
        }

        private static IEnumerable<UInt128> GetRestOfLine(List<UInt128> numbers,
            bool useConcat, int start = 0)
        {
            UInt128 thisNum = numbers[^(start + 1)];

            if (start == numbers.Count - 1)
            {
                yield return thisNum;
            }
            else
            {
                foreach (UInt128 restNum in GetRestOfLine(
                    numbers, useConcat, start + 1))
                {
                    yield return thisNum + restNum;
                    yield return thisNum * restNum;

                    if (useConcat)
                    {
                        string combo = $"{restNum}{thisNum}";
                        UInt128 comboNum = UInt128.Parse(combo);

                        yield return comboNum;
                    }
                }
            }
        }

        private static UInt128 CalculateTotal(
            List<CalcRow> calcRows, bool useConcat)
        {
            UInt128 total = 0;

            foreach (CalcRow row in calcRows)
            {
                foreach (UInt128 possCalc in GetRestOfLine(
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

            UInt128 total = CalculateTotal(calcRows, false);
            UInt128 concatTotal = CalculateTotal(calcRows, true);

            Console.WriteLine($"The total is {total}.");
            Console.WriteLine(
                $"The total with concatenation is {concatTotal}.");
        }
    }
}