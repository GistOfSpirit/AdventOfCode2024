using System.Drawing;
using AoC2024Unified.Solutions.Day15;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day15Solution : IDaySolution
    {
        private const int DayNum = 15;
        private const char WallChar = '#';
        private const char BoxChar = 'O';
        private const char RobotChar = '@';

        private static (
            List<ILocateable> map,
            List<Direction> instructions
        ) ParseInput(string input, bool bigStuff)
        {
            string[] inputParts
                = input.Split(Environment.NewLine + Environment.NewLine,
                    StringSplitOptions.RemoveEmptyEntries);

            string[] matrix = Common.ConvertToMatrix(inputParts[0]);

            List<ILocateable> map = [];

            for (int row = 0; row < matrix.Length; ++row)
            {
                for (int col = 0; col < matrix[row].Length; ++col)
                {
                    int resultCol = bigStuff ? col * 2 : col;
                    Point location = new(resultCol, row);

                    ILocateable? locateable = matrix[row][col] switch
                    {
                        WallChar => new Wall { Location = location },
                        BoxChar => bigStuff
                            ? new BoxWest { Location = location }
                            : new BoxMain { Location = location },
                        RobotChar => new Robot { Location = location },
                        _ => null
                    };

                    if (locateable != null)
                    {
                        map.Add(locateable);
                    }

                    if (locateable is BoxWest)
                    {
                        map.Add(new BoxEast
                        {
                            Location = new Point(
                                locateable.Location.X + 1,
                                locateable.Location.Y
                            )
                        });
                    }
                    else if (locateable is Wall && bigStuff)
                    {
                        map.Add(new Wall
                        {
                            Location = new Point(
                                locateable.Location.X + 1,
                                locateable.Location.Y
                            )
                        });
                    }
                }
            }

            List<Direction> instructions = inputParts[1]
                .Select(Directions.FromChar)
                .Where((c) => c != null)
                .Cast<Direction>()
                .ToList();

            return (map, instructions);
        }

        private static async Task SolveFile(bool isReal, string sub,
            bool bigStuff)
        {
            string input = await Common.ReadFile(isReal, DayNum, sub);

            (List<ILocateable> map,
                List<Direction> instructions) = ParseInput(input, bigStuff);

            Robot robot = (Robot)map.First((l) => l is Robot);

            foreach (Direction instr in instructions)
            {
                robot.Move(map, instr);
            }

            int coordSum = map
                .Where((l) => l is BoxMain)
                .Cast<BoxMain>()
                .Sum((b) => b.Coordinate);

            Console.WriteLine($"The sum of coords is {coordSum}");

            // Visualise(map);
        }

        private static void Visualise(List<ILocateable> map)
        {
            int width = map.Max((l) => l.Location.X) + 1;
            int height = map.Max((l) => l.Location.Y) + 1;

            string[] matrix = new string[height];

            for (int i = 0; i < height; ++i)
            {
                matrix[i] = "".PadRight(width, ' ');
            }

            foreach (ILocateable l in map)
            {
                char c;

                if (l is Wall)
                {
                    c = '#';
                }
                else if (l is Robot)
                {
                    c = '@';
                }
                else if (l is BoxMain)
                {
                    c = 'O';
                }
                else if (l is BoxWest)
                {
                    c = '[';
                }
                else if (l is BoxEast)
                {
                    c = ']';
                }
                else
                {
                    c = '.';
                }

                Common.UpdateMatrix(matrix, l.Location, c);
            }

            foreach (string row in matrix)
            {
                Console.WriteLine(row);
            }
        }

        public async Task Solve(bool isReal)
        {
            if (!isReal)
            {
                await SolveFile(isReal, "a", false);
                await SolveFile(isReal, "b", false);
                await SolveFile(isReal, "c", true);
                await SolveFile(isReal, "b", true);
            }
            else
            {
                await SolveFile(isReal, "", false);
                await SolveFile(isReal, "", true);
            }
        }
    }
}