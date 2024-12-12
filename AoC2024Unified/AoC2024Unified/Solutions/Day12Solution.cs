
using System.Drawing;
using AoC2024Unified.Types;
using AoC2024Unified.Extensions;

namespace AoC2024Unified.Solutions
{
    public class Day12Solution : IDaySolution
    {
        private const int DayNum = 12;

        private class GardenPlot
        {
            public char Fruit { get; set; }
            public List<Point> Locations { get; set; } = [];

            public bool Contains(int row, int col)
                => Locations.Contains(new Point(col, row));

            public int Area => Locations.Count;

            public override string ToString()
                => $"{Fruit} ({Area})";

            public int CalcPerimeter()
            {
                int total = 0;

                foreach (Point l in Locations)
                {
                    foreach (Point adj in
                        Directions.GetCardinal()
                            .Select((d) => d.GetNextPoint(l)))
                    {
                        if (!Locations.Contains(adj))
                        {
                            ++total;
                        }
                    }
                }

                return total;
            }

            public int CalcSides()
            {
                int total = 0;
                var foundFences = new List<(Direction dir, Point p)>();

                foreach (Point l in Locations)
                {
                    foreach (Direction dir in Directions.GetCardinal())
                    {
                        Point adjacentInDir = dir.GetNextPoint(l);

                        if (!Locations.Contains(adjacentInDir))
                        {
                            foundFences.Add((dir, l));

                            var perpPoints = dir.GetPerpendicularPoints(l);

                            if (!perpPoints.Any((pp)
                                => foundFences.Contains((dir, pp))))
                            {
                                ++total;
                            }
                        }
                    }
                }

                return total;
            }
        }

        private static List<GardenPlot> DeterminePlots(string[] map)
        {
            var plots = new List<GardenPlot>();

            for (int row = 0; row < map.Length; ++row)
            {
                for (int col = 0; col < map[row].Length; ++col)
                {
                    char fruit = map[row][col];

                    List<GardenPlot> relPlots = plots.FindAll(
                        (p) => p.Fruit == fruit
                        && (
                            p.Contains(row - 1, col)
                            || p.Contains(row, col - 1)
                        )
                    );

                    GardenPlot plot;

                    if (relPlots.Count == 0)
                    {
                        plot = new GardenPlot { Fruit = fruit };
                        plots.Add(plot);
                    }
                    else
                    {
                        plot = relPlots[0];

                        if (relPlots.Count > 1)
                        {
                            foreach (var otherPlot in relPlots[1..])
                            {
                                plot.Locations.AddRange(otherPlot.Locations);
                                plots.Remove(otherPlot);
                            }
                        }
                    }

                    plot.Locations.Add(new Point(col, row));
                }
            }

            return plots;
        }

        private static string MakeUpGardenName(string[] gardenMap)
            => string.Join("",
                gardenMap.SelectMany((r) => r.ToCharArray()).Distinct());

        private static void ShowFencePrice(string[] gardenMap,
            Func<string[], string> GetName)
        {
            var plots = DeterminePlots(gardenMap);

            int price = 0;
            int discountPrice = 0;

            foreach (GardenPlot p in plots)
            {
                int perim = p.CalcPerimeter();
                price += perim * p.Area;

                int sides = p.CalcSides();
                discountPrice += sides * p.Area;
            }

            Console.WriteLine($"{GetName(gardenMap)}: {price}"
                + $" or {discountPrice}");
        }

        private static async Task SolveTests()
        {
            foreach (string sub in new string[] { "a", "b", "c", "d", "e" })
            {
                string[] gardenMap = await Common.ReadFileAsMatrix(
                    false, DayNum, sub);

                ShowFencePrice(gardenMap, MakeUpGardenName);
            }
        }

        public async Task Solve(bool isReal)
        {
            if (!isReal)
            {
                await SolveTests();
            }
            else
            {
                string[] gardenMap = await Common.ReadFileAsMatrix(
                    true, DayNum);

                ShowFencePrice(gardenMap, (_) => "Real");
            }
        }
    }
}