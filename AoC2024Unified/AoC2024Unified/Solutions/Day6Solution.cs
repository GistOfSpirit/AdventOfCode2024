using System.Drawing;

namespace AoC2024Unified.Solutions
{
    public class Day6Solution : IDaySolution
    {
        private const int DayNum = 6;

        private const char Obstacle = '#';
        private const char GuardNorth = '^';
        private const char GuardSouth = 'v';
        private const char GuardWest = '<';
        private const char GuardEast = '>';
        private const char GuardVisited = 'X';

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

        private static bool WouldMeetMetObstacleIfTurned(
            List<GuardPointDir> metObstacles, char guardDir, Point loc)
            => GetNextGuardDirection(guardDir) switch
            {
                GuardNorth => metObstacles
                    .Any(
                        (o) => o.Direction == GuardNorth
                        && o.X == loc.X
                        && o.Y <= loc.Y
                    ),
                GuardSouth => metObstacles
                    .Any(
                        (o) => o.Direction == GuardSouth
                        && o.X == loc.X
                        && o.Y >= loc.Y
                    ),
                GuardWest => metObstacles
                    .Any(
                        (o) => o.Direction == GuardWest
                        && o.Y == loc.Y
                        && o.X <= loc.X
                    ),
                GuardEast => metObstacles
                    .Any(
                        (o) => o.Direction == GuardEast
                        && o.Y == loc.Y
                        && o.X >= loc.X
                    ),
                _ => throw new InvalidOperationException("Invalid guard char")
            };

        private static bool AdvanceGuard(string[] map, ref Point loc,
            List<GuardPointDir> metObstacles, List<Point> possRetconSpots)
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
                return true;
            }
            else if (HasObstacle(map, newLoc))
            {
                metObstacles.Add(new GuardPointDir(loc, guard));
                TurnGuard(map, loc);
            }
            else
            {
                if (WouldMeetMetObstacleIfTurned(metObstacles, guard, loc))
                {
                    possRetconSpots.Add(newLoc);
                }

                Common.UpdateMatrix(map, loc, GuardVisited);
                Common.UpdateMatrix(map, newLoc, guard);
                loc = newLoc;
            }

            return false;
        }

        private static int CountVisited(string[] map)
            => map.Sum((r) => r.Count((c) => c == GuardVisited));

        public async Task Solve(bool isReal)
        {
            string[] map = await Common.ReadFileAsMatrix(isReal, DayNum);

            Point guardLoc = LocateGuard(map);

            var metObstacles = new List<GuardPointDir>();
            var possRetconSpots = new List<Point>();

            while (!AdvanceGuard(
                map, ref guardLoc, metObstacles, possRetconSpots))
            { }

            int totalVisited = CountVisited(map);

            Console.WriteLine($"The guard has visited {totalVisited} spots");
            Console.WriteLine(
                $"Possible retcon spots: {possRetconSpots.Count}");
        }
    }
}