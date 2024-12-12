using System.ComponentModel;
using System.Drawing;

namespace AoC2024Unified.Types
{
    public enum Direction
    {
        North,
        Northeast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        Northwest
    }

    public static class Directions
    {
        public static List<Direction> GetPerpendicular(this Direction dir)
            => dir switch
            {
                Direction.North => [Direction.West, Direction.East],
                Direction.Northeast
                    => [Direction.Northwest, Direction.Southeast],
                Direction.East => [Direction.North, Direction.South],
                Direction.Southeast
                    => [Direction.Northeast, Direction.Southwest],
                Direction.South => [Direction.East, Direction.West],
                Direction.Southwest
                    => [Direction.Southeast, Direction.Northwest],
                Direction.West => [Direction.South, Direction.North],
                Direction.Northwest
                    => [Direction.Southwest, Direction.Northeast],
                _ => throw new InvalidEnumArgumentException("Invalid direction")
            };

        public static Point GetNextPoint(this Direction dir, Point p)
            => dir switch
            {
                Direction.North => new Point(p.X, p.Y - 1),
                Direction.Northeast => new Point(p.X + 1, p.Y - 1),
                Direction.East => new Point(p.X + 1, p.Y),
                Direction.Southeast => new Point(p.X + 1, p.Y + 1),
                Direction.South => new(p.X, p.Y + 1),
                Direction.Southwest => new(p.X - 1, p.Y + 1),
                Direction.West => new(p.X - 1, p.Y),
                Direction.Northwest => new(p.X - 1, p.Y - 1),
                _ => throw new InvalidEnumArgumentException("Invalid direction")
            };

        public static List<Point> GetPerpendicularPoints(
            this Direction dir, Point p)
            => dir.GetPerpendicular().Select((d) => d.GetNextPoint(p)).ToList();

        public static List<Direction> GetCardinal()
            => [Direction.North, Direction.East,
                Direction.South, Direction.West];

        public static List<Direction> GetSoutheastHalf()
            => [Direction.Northeast, Direction.East,
                Direction.South, Direction.Southeast];
    }
}