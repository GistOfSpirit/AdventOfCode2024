using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AoC2024Unified.Types
{
    public struct MarkedPoint(Point loc, char mark)
        : IEquatable<MarkedPoint>
    {
        public int X { get; set; } = loc.X;
        public int Y { get; set; } = loc.Y;

        public char Mark { get; set; } = mark;

        public static List<MarkedPoint> ParseMatrix(string[] matrix,
            Func<char, bool> isValid)
        {
            var points = new List<MarkedPoint>();

            for (int row = 0; row < matrix.Length; ++row)
            {
                for (int col = 0; col < matrix[row].Length; ++col)
                {
                    if (isValid(matrix[row][col]))
                    {
                        points.Add(new MarkedPoint
                        {
                            X = col,
                            Y = row,
                            Mark = matrix[row][col]
                        });
                    }
                }
            }

            return points;
        }

        public readonly bool Equals(MarkedPoint other)
            => X == other.X && Y == other.Y && Mark == other.Mark;

        public override readonly bool Equals([NotNullWhen(true)] object? obj)
            => obj is MarkedPoint other && Equals(other);

        public override readonly int GetHashCode()
            => (X, Y, Mark).GetHashCode();

        public static bool operator ==(MarkedPoint lhs, MarkedPoint rhs)
            => lhs.Equals(rhs);

        public static bool operator !=(MarkedPoint lhs, MarkedPoint rhs)
            => !(lhs == rhs);

        public static Size operator -(MarkedPoint lhs, MarkedPoint rhs)
            => new(lhs.X - rhs.X, lhs.Y - rhs.Y);

        public static Point operator +(MarkedPoint lhs, Size rhs)
            => new(lhs.X + rhs.Width, lhs.Y + rhs.Height);

        public static Point operator -(MarkedPoint lhs, Size rhs)
            => new(lhs.X - rhs.Width, lhs.Y - rhs.Height);

        public readonly Point AsPoint() => new(X, Y);

        public override readonly string ToString()
            => $"{Mark}: ({X},{Y})";
    }
}