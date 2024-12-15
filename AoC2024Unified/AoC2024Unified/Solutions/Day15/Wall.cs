using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public class Wall : ILocateable
    {
        public Point Location { get; set; }
        public int Width { get; set; }

        public bool CanMove(List<ILocateable> map, Direction direction)
            => false;

        public void Move(List<ILocateable> map, Direction direction)
            => throw new InvalidOperationException("Tried to move a wall");
    }
}