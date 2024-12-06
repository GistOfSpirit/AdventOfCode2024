using System.Drawing;

namespace AoC2024Unified.Solutions
{
    public class Day6Solution : IDaySolution
    {
        private const int DayNum = 6;

        private const char Empty = '.';
        private const char Obstacle = '#';
        private const char GuardNorth = '^';
        private const char GuardSouth = 'v';
        private const char GuardWest = '<';
        private const char GuardEast = '>';
        private const char GuardVisited = 'X';

        private enum AdvancementResult
        {
            InMap,
            OffMap,
            Revisit
        }

        private struct GuardPointDir
        {
            public int X { get; set; }
            public int Y { get; set; }

            private char _direction;

            public char Direction
            {
                get => _direction;
                set
                {
                    if (
                        value != GuardNorth
                        && value != GuardSouth
                        && value != GuardWest
                        && value != GuardEast
                    )
                    {
                        throw new InvalidOperationException(
                            "Invalid guard char");
                    }

                    _direction = value;
                }
            }

            public GuardPointDir(Point loc, char dir)
            {
                X = loc.X;
                Y = loc.Y;
                Direction = dir;
            }
        }

        private static Point LocateGuard(string[] map)
        {
            for (int row = 0; row < map.Length; ++row)
            {
                int col = map[row].IndexOfAny(
                    [GuardNorth, GuardSouth, GuardWest, GuardEast]);

                if (col >= 0)
                {
                    return new Point(col, row);
                }
            }

            throw new InvalidOperationException("Guard not found");
        }

        private static bool HasObstacle(string[] map, Point loc)
            => map[loc.Y][loc.X] == Obstacle;

        private static char GetNextGuardDirection(char guard)
            => guard switch
            {
                GuardNorth => GuardEast,
                GuardSouth => GuardWest,
                GuardWest => GuardNorth,
                GuardEast => GuardSouth,
                _ => throw new InvalidOperationException(
                    "Invalid guard char")
            };

        private static void TurnGuard(string[] map, Point loc)
        {
            char guard = map[loc.Y][loc.X];

            var newGuard = GetNextGuardDirection(guard);

            Common.UpdateMatrix(map, loc, newGuard);
        }

        private static bool IsOutsideMap(string[] map, Point loc)
            => loc.X < 0 || loc.X >= map[0].Length
                || loc.Y < 0 || loc.Y >= map.Length;

        private static AdvancementResult AdvanceGuard(string[] map,
            List<GuardPointDir> metObstacles, ref Point loc)
        {
            char guard = map[loc.Y][loc.X];

            var newLoc = guard switch
            {
                GuardNorth => new Point(loc.X, loc.Y - 1),
                GuardSouth => new Point(loc.X, loc.Y + 1),
                GuardWest => new Point(loc.X - 1, loc.Y),
                GuardEast => new Point(loc.X + 1, loc.Y),
                _ => throw new InvalidOperationException(
                    "Guard not where expected")
            };

            if (IsOutsideMap(map, newLoc))
            {
                Common.UpdateMatrix(map, loc, GuardVisited);
                return AdvancementResult.OffMap;
            }
            else if (HasObstacle(map, newLoc))
            {
                // Local non-ref copy to use in lambda
                Point location = loc;

                if (metObstacles.Any(
                    (o) => o.Direction == guard
                    && o.X == location.X
                    && o.Y == location.Y
                ))
                {
                    return AdvancementResult.Revisit;
                }

                metObstacles.Add(new GuardPointDir(loc, guard));
                TurnGuard(map, loc);
            }
            else
            {
                Common.UpdateMatrix(map, loc, GuardVisited);
                Common.UpdateMatrix(map, newLoc, guard);
                loc = newLoc;
            }

            return AdvancementResult.InMap;
        }

        private static int CountVisited(string[] map)
            => map.Sum((r) => r.Count((c) => c == GuardVisited));

        private IEnumerable<string[]> AddOneObstacle(string[] map)
        {
            for (int row = 0; row < map.Length; ++row)
            {
                for (int col = 0; col < map[row].Length; ++col)
                {
                    if (map[row][col] == Empty)
                    {
                        string[] newMap = (string[])map.Clone();
                        Common.UpdateMatrix(
                            newMap, new Point(col, row), Obstacle);
                        yield return newMap;
                    }
                }
            }
        }

        public async Task Solve(bool isReal)
        {
            string[] map = await Common.ReadFileAsMatrix(isReal, DayNum);
            string[] mapOrig = (string[])map.Clone();

            Point guardLoc = LocateGuard(map);
            Point guardLogOrig = guardLoc;

            var metObstacles = new List<GuardPointDir>();

            AdvancementResult result;

            do
            {
                result = AdvanceGuard(map, metObstacles, ref guardLoc);
            } while (result == AdvancementResult.InMap);

            if (result == AdvancementResult.Revisit)
            {
                throw new InvalidOperationException("Revisit on unedited path");
            }

            int totalVisited = CountVisited(map);

            var retconSpots = new List<Point>();

            TimeSpan bTime;
            DateTime start = DateTime.Now;

            foreach (string[] testMap in AddOneObstacle(mapOrig))
            {
                AdvancementResult retconResult;
                Point testGuardLoc = guardLogOrig;
                var testMetObstacles = new List<GuardPointDir>();

                do
                {
                    retconResult = AdvanceGuard(
                        testMap, testMetObstacles, ref testGuardLoc);
                } while (retconResult == AdvancementResult.InMap);


                if (retconResult == AdvancementResult.Revisit)
                {
                    retconSpots.Add(guardLoc);
                }
            }

            DateTime end = DateTime.Now;
            bTime = end - start;

            Console.WriteLine($"The guard has visited {totalVisited} spots");
            Console.WriteLine(
                $"Possible retcon spots: {retconSpots.Count}."
                + $"(Took {bTime.TotalSeconds} seconds)");
        }
    }
}