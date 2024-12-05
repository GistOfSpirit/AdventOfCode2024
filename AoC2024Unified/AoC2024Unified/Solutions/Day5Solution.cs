using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day5Solution : IDaySolution
    {
        const int DayNum = 5;

        private static List<int> GetPagesAfter(NumberRowList rules, int page)
            => rules.Where((r) => r[0] == page)
                .Select((r) => r[1])
                .ToList();

        private static bool IsRowCorrect(NumberRowList rules, List<int> row)
        {
            for (int i = 0; i < row.Count; ++i)
            {
                var pagesAfter = GetPagesAfter(rules, row[i]);

                foreach (int otherPage in pagesAfter)
                {
                    if (
                        row.IndexOf(otherPage) >= 0
                        && row.IndexOf(otherPage) < i
                    )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static List<int> FixRow(NumberRowList rules, List<int> row)
        {
            var fixedRow = new List<int>(row);

            fixedRow.Sort((x, y) =>
            {
                var xMustBeAfter = rules
                    .Where((r) => r[1] == x)
                    .Select((r) => r[0])
                    .ToList();

                if (xMustBeAfter.Contains(y))
                {
                    return 1;
                }

                var xMustBeBefore = rules
                    .Where((r) => r[0] == x)
                    .Select((r) => r[1])
                    .ToList();

                if (xMustBeBefore.Contains(y))
                {
                    return -1;
                }

                return 0;
            });

            return fixedRow;
        }

        private static (
            NumberRowList correctRows,
            NumberRowList incorrectRows
        ) SplitCorrectRows(Day5Input input)
        {
            var correctRows = new NumberRowList();
            var incorrectRows = new NumberRowList();

            foreach (List<int> row in input.PageUpdates)
            {
                if (IsRowCorrect(input.OrderingRules, row))
                {
                    correctRows.Add(row);
                }
                else
                {
                    incorrectRows.Add(row);
                }
            }

            return (correctRows, incorrectRows);
        }

        private static int GetRowMiddle(List<int> row)
            => row[row.Count / 2];

        public async Task Solve(bool isReal)
        {
            string inputText = await Common.ReadFile(isReal, DayNum);

            var inputData = Day5Input.Parse(inputText);

            var (correctRows, incorrectRows) = SplitCorrectRows(inputData);

            int totalOfCorrect = correctRows.Sum(GetRowMiddle);

            NumberRowList fixedRows = [..incorrectRows
                .Select((r) => FixRow(inputData.OrderingRules, r))
                .ToList()];

            int totalOfFixed = fixedRows.Sum(GetRowMiddle);

            Console.WriteLine($"The total of middles is {totalOfCorrect}");
            Console.WriteLine($"The total of fixed middles is {totalOfFixed}");
        }
    }
}