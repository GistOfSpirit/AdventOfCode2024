using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day2Solution : IDaySolution
    {
        private static bool OnlyIncreasing(IList<int> intList)
        {
            for (int i = 0; i < intList.Count - 1; ++i)
            {
                if (intList[i + 1] < intList[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool OnlyDecreasing(IList<int> intList)
        {
            for (int i = 0; i < intList.Count - 1; ++i)
            {
                if (intList[i + 1] > intList[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static int GetMaxDiff(IList<int> intList)
        {
            int maxDiff = 0;

            for (int i = 0; i < intList.Count - 1; ++i)
            {
                maxDiff = Math.Max(maxDiff,
                    Math.Abs(intList[i + 1] - intList[i]));
            }

            return maxDiff;
        }

        private static int GetMinDiff(IList<int> intList)
        {
            int minDiff = Math.Abs(intList[1] - intList[0]);

            for (int i = 1; i < intList.Count - 1; ++i)
            {
                minDiff = Math.Min(minDiff,
                    Math.Abs(intList[i + 1] - intList[i]));
            }

            return minDiff;
        }

        private static bool IsRowSafe(IList<int> intList)
        {
            return (
                    OnlyIncreasing(intList)
                    || OnlyDecreasing(intList)
                )
                && (GetMinDiff(intList) >= 1)
                && (GetMaxDiff(intList) <= 3);
        }

        private static IEnumerable<IList<int>> GetProblemDampenerOptions(
            IList<int> intList)
        {
            for (int ignore = 0; ignore < intList.Count; ++ignore)
            {
                var newList = new List<int>(intList);

                newList.RemoveAt(ignore);

                yield return newList;
            }
        }

        private const int DayNum = 2;

        public async Task Solve(bool isReal)
        {
            NumberRowList rowList = await Common.ReadNumberRows(
                $"{DayNum}", isReal);

            int safeRowCount = 0;
            int safePDRowCount = 0;

            foreach (IList<int> intList in rowList)
            {
                if (IsRowSafe(intList))
                {
                    ++safeRowCount;
                    ++safePDRowCount;
                }
                else
                {
                    foreach (
                        IList<int> pdList in GetProblemDampenerOptions(intList))
                    {
                        if (IsRowSafe(pdList))
                        {
                            ++safePDRowCount;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine($"There are {safeRowCount} safe rows.");
            Console.WriteLine($"There are {safePDRowCount} PD safe rows.");
        }
    }
}