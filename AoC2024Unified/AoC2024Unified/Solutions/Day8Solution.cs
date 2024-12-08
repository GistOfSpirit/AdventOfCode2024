using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day8Solution : IDaySolution
    {
        private const int DayNum = 8;

        private static int CountAntinodes(
            List<MarkedPoint> antennae, Rectangle matrixSize)
        {
            var antinodes = new List<Point>();

            List<char> antennaTypes = antennae
                .Select((a) => a.Mark)
                .Distinct()
                .ToList();

            foreach (char antennaType in antennaTypes)
            {
                List<MarkedPoint> relevantAntennae = antennae
                    .Where((a) => a.Mark == antennaType)
                    .ToList();

                for (int x = 0; x < relevantAntennae.Count; ++x)
                {
                    MarkedPoint relAnt1 = relevantAntennae[x];

                    for (int y = x; y < relevantAntennae.Count; ++y)
                    {
                        MarkedPoint relAnt2 = relevantAntennae[y];

                        if (relAnt1 != relAnt2)
                        {
                            Size distance = relAnt1 - relAnt2;
                            Point an1 = relAnt1 + distance;
                            Point an2 = relAnt2 - distance;

                            if (matrixSize.Contains(an1))
                            {
                                antinodes.Add(an1);
                            }

                            if (matrixSize.Contains(an2))
                            {
                                antinodes.Add(an2);
                            }
                        }
                    }
                }
            }

            return antinodes.Distinct().Count();
        }

        public async Task Solve(bool isReal)
        {
            string[] matrix = await Common.ReadFileAsMatrix(isReal, DayNum);

            List<MarkedPoint> antennae = MarkedPoint.ParseMatrix(
                matrix, char.IsLetterOrDigit);

            int antinodeCount = CountAntinodes(antennae,
                new Rectangle(0, 0, matrix[0].Length, matrix.Length));

            Console.WriteLine($"Found {antinodeCount} antinodes.");
        }
    }
}