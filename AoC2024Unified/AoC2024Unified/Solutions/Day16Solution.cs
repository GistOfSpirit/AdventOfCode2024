using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public class Day16Solution : IDaySolution
    {
        private const int DayNum = 16;
        private const int MoveScore = 1;
        private const int TurnScore = 1000;
        private const Direction InitDirection = Direction.East;
        private const char WallChar = '#';
        private const char StartChar = 'S';
        private const char EndChar = 'E';

        public class Maze
        {
            public Point StartPoint { get; set; }
            public Point EndPoint { get; set; }
            public List<Point> Walls { get; set; } = [];
        }

        private static List<List<Point>> FindPaths(Maze maze,
            List<Point> pastPoints)
        {
            if (pastPoints.Count == 0)
            {
                pastPoints.Add(maze.StartPoint);
            }

            Point currLocation = pastPoints[^1];

            List<List<Point>> futurePaths = [];

            foreach (Direction dir in Directions.GetCardinal())
            {
                Point nextPoint = dir.GetNextPoint(currLocation);

                if (
                    maze.Walls.Contains(nextPoint)
                    || pastPoints.Contains(nextPoint)
                )
                {
                    continue;
                }

                List<Point> newPastPoints = [.. pastPoints];
                newPastPoints.Add(nextPoint);

                if (nextPoint == maze.EndPoint)
                {
                    return [newPastPoints];
                }

                futurePaths.AddRange(FindPaths(maze, newPastPoints));
            }

            return futurePaths;
        }

        private static Maze ParseInput(string[] matrix)
        {
            Maze maze = new();

            for (int row = 0; row < matrix.Length; ++row)
            {
                for (int col = 0; col < matrix[row].Length; ++col)
                {
                    char here = matrix[row][col];

                    if (here == WallChar)
                    {
                        maze.Walls.Add(new Point(col, row));
                    }
                    else if (here == StartChar)
                    {
                        maze.StartPoint = new Point(col, row);
                    }
                    else if (here == EndChar)
                    {
                        maze.EndPoint = new Point(col, row);
                    }
                }
            }

            return maze;
        }

        private static int ScorePath(List<Point> path)
        {
            Direction currDir = InitDirection;
            Point currPoint = path[0];
            int score = 0;

            foreach (Point point in path[1..])
            {
                if (point != currDir.GetNextPoint(currPoint))
                {
                    score += TurnScore;

                    Direction? newDir = Directions.GetFromTo(currPoint, point);

                    if (
                        newDir == null
                        || !newDir.Value.IsCardinal()
                    )
                    {
                        throw new Exception("Invalid move");
                    }

                    currDir = newDir.Value;
                }

                score += MoveScore;
                currPoint = point;
            }

            return score;
        }

        private static async Task SolveOne(bool isReal, string sub)
        {
            string[] matrix = await Common.ReadFileAsMatrix(
                isReal, DayNum, sub);

            Maze maze = ParseInput(matrix);

            List<List<Point>> paths = FindPaths(maze, []);

            int lowestScore = ScorePath(paths[0]);

            foreach (List<Point> path in paths[1..])
            {
                lowestScore = Math.Min(lowestScore, ScorePath(path));
            }

            Console.WriteLine($"Lowest score is {lowestScore}");
        }

        public async Task Solve(bool isReal)
        {
            if (!isReal)
            {
                await SolveOne(isReal, "a");
                await SolveOne(isReal, "b");
            }
            else
            {
                await SolveOne(isReal, "");
            }
        }
    }
}