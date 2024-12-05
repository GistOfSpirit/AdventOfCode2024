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

        private static NumberRowList GetCorrectRows(Day5Input input)
            => new(input.PageUpdates
                .Where((u) => IsRowCorrect(input.OrderingRules, u))
                .ToList());

        private static int GetRowMiddle(List<int> row)
            => row[row.Count / 2];

        public async Task Solve(bool isReal)
        {
            string inputText = await Common.ReadFile(isReal, DayNum);

            var inputData = Day5Input.Parse(inputText);

            NumberRowList correctRows = GetCorrectRows(inputData);

            int total = correctRows.Sum(GetRowMiddle);

            Console.WriteLine($"The total of middles is {total}");
        }
    }
}