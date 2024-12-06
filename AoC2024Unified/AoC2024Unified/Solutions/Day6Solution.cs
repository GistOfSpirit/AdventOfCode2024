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

        private static void TurnGuard(string[] map, Point loc)
        {
            char guard = map[loc.Y][loc.X];

            var newGuard = guard switch
            {
                GuardNorth => GuardEast,
                GuardSouth => GuardWest,
                GuardWest => GuardNorth,
                GuardEast => GuardSouth,
                _ => throw new InvalidOperationException(
                    "Guard not where expected")
            };

            Common.UpdateMatrix(map, loc, newGuard);
        }

        private static bool IsOutsideMap(string[] map, Point loc)
            => loc.X < 0 || loc.X >= map[0].Length
                || loc.Y < 0 || loc.Y >= map.Length;

        private static bool AdvanceGuard(string[] map, ref Point loc)
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
                TurnGuard(map, loc);
            }
            else
            {
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

            while (!AdvanceGuard(map, ref guardLoc)) { }

            int total = CountVisited(map);

            Console.WriteLine($"The guard has visited {total} spots");
        }
    }
}