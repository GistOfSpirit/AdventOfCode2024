
using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC2024Unified.Solutions
{
    public class Day14Solution : IDaySolution
    {
        private const int DayNum = 14;
        private const int AreaWidthTest = 11;
        private const int AreaHeightTest = 7;
        private const int AreaWidthReal = 101;
        private const int AreaHeightReal = 103;
        private const int SecondsToAdvance = 100;

        private class Robot
        {
            public Point Position { get; set; }
            public Size Velocity { get; set; }

            private static int ConstrainTo(int value, int areaSize)
            {
                int result = value;

                if (result < 0)
                {
                    result = (result % areaSize) + areaSize;
                }

                result %= areaSize;

                return result;
            }

            public void Advance(int seconds, Size area)
            {
                var currPos = Position;

                currPos += Velocity * seconds;
                currPos.X = ConstrainTo(currPos.X, area.Width);
                currPos.Y = ConstrainTo(currPos.Y, area.Height);

                Position = currPos;
            }
        }

        private static List<Robot> ParseInput(string input)
        {
            const string pattern = @"p=(?<px>[0-9]+),(?<py>[0-9]+) "
                + @"v=(?<vx>-?[0-9]+),(?<vy>-?[0-9]+)";

            var matches = Regex.Matches(input, pattern,
                RegexOptions.None, TimeSpan.FromSeconds(3));

            var robots = new List<Robot>();

            foreach (Match match in matches)
            {
                int px = int.Parse(match.Groups["px"].Value);
                int py = int.Parse(match.Groups["py"].Value);
                int vx = int.Parse(match.Groups["vx"].Value);
                int vy = int.Parse(match.Groups["vy"].Value);

                robots.Add(new Robot
                {
                    Position = new Point(px, py),
                    Velocity = new Size(vx, vy)
                });
            }

            return robots;
        }

        private static int[] CountRobotsInQuadrants(
            List<Robot> robots, Size area)
        {
            int xMid = area.Width / 2;
            int yMid = area.Height / 2;

            var quads = new List<Rectangle>
            {
                new(0, 0, xMid, yMid),
                new(xMid + 1, 0, xMid, yMid),
                new(0, yMid + 1, xMid, yMid),
                new(xMid + 1, yMid + 1, xMid, yMid)
            };

            var counts = new int[4];

            foreach (Robot robot in robots)
            {
                for (int i = 0; i < quads.Count; ++i)
                {
                    if (quads[i].Contains(robot.Position))
                    {
                        ++counts[i];
                    }
                }
            }

            return counts;
        }

        private static bool HasTABottomLine(List<Robot> robots, Size area)
            => Enumerable.Range(0, area.Width)
                .All((x) => robots
                    .Any((r) => r.Position == new Point(x, area.Height - 1)));

        private static bool HasTAMiddleOfTop(List<Robot> robots, Size area)
            => robots.Any((r) => (r.Position.Y == 1)
                && (r.Position.X == area.Width / 2));

        private static bool HasTASides(List<Robot> robots, Size area)
            => Enumerable.Range(2, area.Height - 1)
                .All((y) => robots.Any(
                        (r) => r.Position == new Point(
                            (area.Width / 2) - y, y))
                    && robots.Any(
                        (r) => r.Position == new Point(
                            (area.Width / 2) + y, y)));

        private static bool IsTreeangle(List<Robot> robots, Size area)
            => HasTABottomLine(robots, area)
                && HasTAMiddleOfTop(robots, area)
                && HasTASides(robots, area);

        private static int FindSecondWithTreeangle(
            List<Robot> robots, Size area)
        {
            DateTime startTime = DateTime.Now;
            int seconds = 0;

            do
            {
                if (IsTreeangle(robots, area))
                {
                    return seconds;
                }

                foreach (Robot robot in robots)
                {
                    robot.Advance(1, area);
                }

                checked
                {
                    ++seconds;
                }
            } while ((DateTime.Now - startTime).TotalSeconds < 60);

            return -seconds;
        }

        private static void TestIntentionalTreeangle()
        {
            var area = new Size(4, 7);
            var robots = new List<Robot>();

            // Fill bottom line
            for (int x = 0; x < area.Width; ++x)
            {
                robots.Add(new Robot
                {
                    Position = new Point(x, area.Height - 1),
                    Velocity = new Size(0, 0)
                });
            }

            // Put at top
            robots.Add(new Robot
            {
                Position = new Point(area.Width / 2),
                Velocity = new Size(0, 0)
            });

            // Put at sides
            for (int y = 1; y < area.Height; ++y)
            {
                robots.Add(new Robot
                {
                    Position = new Point(area.Width / 2 - y, y),
                    Velocity = new Size(0, 0)
                });

                robots.Add(new Robot
                {
                    Position = new Point(area.Width / 2 + y, y),
                    Velocity = new Size(0, 0)
                });
            }

            var a = HasTABottomLine(robots, area);
            var b = HasTAMiddleOfTop(robots, area);
            var c = HasTASides(robots, area);
        }

        private static void Visualise(List<Robot> robots, Size area)
        {
            string[] screen = new string[area.Height];

            for (int i = 0; i < area.Height; ++i)
            {
                screen[i] = "".PadRight(area.Width, ' ');
            }

            foreach (Robot robot in robots)
            {
                Common.UpdateMatrix(screen, robot.Position, '*');
            }

            Console.Clear();

            foreach (string row in screen)
            {
                Console.WriteLine(row);
            }
        }

        private static int StartingConditionRepeats(List<Robot> robots,
            Size area, List<Point> origPoints)
        {
            int seconds = 0;

            while (true)
            {
                foreach (Robot robot in robots)
                {
                    robot.Advance(1, area);
                }

                if (robots.All((r) => origPoints.Any((p) => r.Position == p)))
                {
                    break;
                }

                ++seconds;
            }

            return seconds;
        }

        public async Task Solve(bool isReal)
        {
            string input = await Common.ReadFile(isReal, DayNum);
            List<Robot> robots = ParseInput(input);

            Size area = isReal
                ? new Size(AreaWidthReal, AreaHeightReal)
                : new Size(AreaWidthTest, AreaHeightTest);

            foreach (Robot robot in robots)
            {
                robot.Advance(SecondsToAdvance, area);
            }

            int[] counts = CountRobotsInQuadrants(robots, area);

            int safetyFactor = counts.Aggregate((x, y) => x * y);

            Console.WriteLine($"The safety factor is {safetyFactor}");

            if (isReal)
            {
                var newRobots = ParseInput(input);
                var origPoints = newRobots.Select((r) => r.Position).ToList();

                int seconds = 0;

                // Solution found by visual inspection! 7083
                while (true)
                {
                    Visualise(newRobots, area);
                    Console.WriteLine($"{seconds}");

                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.RightArrow)
                    {
                        foreach (Robot robot in newRobots)
                        {
                            robot.Advance(1, area);
                        }

                        ++seconds;
                    }
                    else if (key.Key == ConsoleKey.LeftArrow)
                    {
                        foreach (Robot robot in newRobots)
                        {
                            robot.Advance(-1, area);
                        }

                        --seconds;
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
            }
        }
    }
}