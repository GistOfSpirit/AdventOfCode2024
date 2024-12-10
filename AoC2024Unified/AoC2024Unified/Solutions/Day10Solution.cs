
using System.Drawing;

namespace AoC2024Unified.Solutions
{
    public class Day10Solution : IDaySolution
    {
        private const int DayNum = 10;

        private const int TrailHeadHeight = 0;
        private const int PeakHeight = 9;

        private static List<Point> LocateTrailheads(string[] map)
        {
            var list = new List<Point>();

            for (int row = 0; row < map.Length; ++row)
            {
                for (int col = 0; col < map[row].Length; ++col)
                {
                    if (GetPositionHeight(map, col, row) == TrailHeadHeight)
                    {
                        list.Add(new Point(col, row));
                    }
                }
            }

            return list;
        }

        private static int GetPositionHeight(string[] map, int x, int y)
            => int.Parse(map[y][x].ToString());

        private static int GetPositionHeight(string[] map, Point pos)
            => GetPositionHeight(map, pos.X, pos.Y);

        private static List<Point> GetPossiblePaths(string[] map, Point pos)
        {
            var paths = new List<Point>();

            int currHeight = GetPositionHeight(map, pos);

            if (
                pos.X > 0
                && GetPositionHeight(map, pos.X - 1, pos.Y) == currHeight + 1
            )
            {
                paths.Add(new Point(pos.X - 1, pos.Y));
            }

            if (
                pos.X < map[pos.Y].Length - 1
                && GetPositionHeight(map, pos.X + 1, pos.Y) == currHeight + 1
            )
            {
                paths.Add(new Point(pos.X + 1, pos.Y));
            }

            if (
                pos.Y > 0
                && GetPositionHeight(map, pos.X, pos.Y - 1) == currHeight + 1
            )
            {
                paths.Add(new Point(pos.X, pos.Y - 1));
            }

            if (
                pos.Y < map.Length - 1
                && GetPositionHeight(map, pos.X, pos.Y + 1) == currHeight + 1
            )
            {
                paths.Add(new Point(pos.X, pos.Y + 1));
            }

            return paths;
        }

        public static List<Point> GetReachablePeaks(string[] map, Point pos,
            bool distinct)
        {
            List<Point> possPaths = GetPossiblePaths(map, pos);

            if (possPaths.Count == 0)
            {
                return [];
            }

            if (GetPositionHeight(map, pos) == PeakHeight - 1)
            {
                return possPaths;
            }

            var peaksFuture = new List<Point>();

            foreach (Point path in possPaths)
            {
                peaksFuture.AddRange(GetReachablePeaks(map, path, distinct));
            }

            if (distinct)
            {
                return peaksFuture.Distinct().ToList();
            }
            else
            {
                return peaksFuture;
            }
        }

        private static int CalcTrailheadScore(string[] map, Point trailhead,
            bool distinct)
            => GetReachablePeaks(map, trailhead, distinct).Count;

        private static int CalcMapScore(string[] map, bool distinct)
            => LocateTrailheads(map)
                .Sum((th) => CalcTrailheadScore(map, th, distinct));

        public async Task Solve(bool isReal)
        {
            string[] map = await Common.ReadFileAsMatrix(isReal, DayNum);

            int mapScore = CalcMapScore(map, true);

            Console.WriteLine($"The map's score is {mapScore}");

            int ratingSum = CalcMapScore(map, false);

            Console.WriteLine($"The sum of ratings is {ratingSum}");
        }
    }
}