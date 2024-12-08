using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day8Solution : IDaySolution
    {
        private const int DayNum = 8;

        private static int CountAntinodesA(
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

                    for (int y = x + 1; y < relevantAntennae.Count; ++y)
                    {
                        MarkedPoint relAnt2 = relevantAntennae[y];

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

            return antinodes.Distinct().Count();
        }

        private static int GCD(int a, int b)
        {
            // "Stolen" code
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        private static int CountAntinodesB(
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

                    for (int y = x + 1; y < relevantAntennae.Count; ++y)
                    {
                        MarkedPoint relAnt2 = relevantAntennae[y];

                        Size distance = relAnt1 - relAnt2;
                        int minDistCoord = GCD(
                            Math.Abs(distance.Width),
                            Math.Abs(distance.Height));
                        Size step = distance / minDistCoord;

                        for (Point i = relAnt1.AsPoint();
                            matrixSize.Contains(i);
                            i += step
                        )
                        {
                            antinodes.Add(i);
                        }

                        for (
                            Point i = relAnt1.AsPoint() - step;
                            matrixSize.Contains(i);
                            i -= step
                        )
                        {
                            antinodes.Add(i);
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

            var matrixSize = new Rectangle(
                0, 0, matrix[0].Length, matrix.Length);

            int antinodeCount = CountAntinodesA(antennae, matrixSize);

            Console.WriteLine($"Found {antinodeCount} antinodes.");

            int harmonicAnCount = CountAntinodesB(antennae, matrixSize);

            Console.WriteLine(
                $"Found {harmonicAnCount} antinodes with harmonics.");
        }
    }
}