
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
        }
    }
}