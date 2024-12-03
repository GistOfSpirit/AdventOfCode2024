using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day1Solution : IDaySolution
    {
        private const int DayNum = 1;

        public async Task Solve(bool isReal)
        {
            NumberRowList rowList = await Common.ReadNumberRows(DayNum, isReal);

            var leftList = new List<int>();
            var rightList = new List<int>();

            foreach (var numRow in rowList)
            {
                leftList.Add(Convert.ToInt32(numRow[0]));
                rightList.Add(Convert.ToInt32(numRow[1]));
            }

            if (leftList.Count != rightList.Count)
            {
                throw new InvalidOperationException("Invalid list count");
            }

            leftList.Sort();
            rightList.Sort();

            int totalDistance = 0;
            int similarityScore = 0;

            for (int i = 0; i < leftList.Count; ++i)
            {
                int leftNum = leftList[i];
                int rightNum = rightList[i];

                int distance = Math.Abs(leftNum - rightNum);

                totalDistance += distance;

                int rightCount = rightList.Count((n) => n == leftNum);
                int similarity = leftNum * rightCount;

                similarityScore += similarity;
            }

            Console.WriteLine($"Total distance is: {totalDistance}");
            Console.WriteLine($"Similarity is: {similarityScore}");
        }
    }
}