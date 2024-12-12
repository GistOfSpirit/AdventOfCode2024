
using System.Drawing;

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

            private static List<Point> GetAdjacent(Point p)
                => [
                    new(p.X + 1, p.Y),
                    new(p.X - 1, p.Y),
                    new(p.X, p.Y + 1),
                    new(p.X, p.Y - 1)
                ];

            public int CalcPerimeter()
            {
                int total = 0;

                foreach (Point l in Locations)
                {
                    foreach (Point adj in GetAdjacent(l))
                    {
                        if (!Locations.Contains(adj))
                        {
                            ++total;
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

        private static void ShowFencePrice(string[] gardenMap)
        {
            var plots = DeterminePlots(gardenMap);

            int price = 0;

            foreach (GardenPlot p in plots)
            {
                int perim = p.CalcPerimeter();
                price += perim * p.Area;
            }

            Console.WriteLine($"The price is {price}");
        }

        private static async Task SolveTests()
        {
            foreach (string sub in new string[] { "a", "b", "c" })
            {
                string[] gardenMap = await Common.ReadFileAsMatrix(
                    false, DayNum, sub);

                ShowFencePrice(gardenMap);
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

                ShowFencePrice(gardenMap);
            }
        }
    }
}