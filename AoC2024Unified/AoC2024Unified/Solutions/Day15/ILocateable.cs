using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public interface ILocateable
    {
        Point Location { get; set; }
        int Width { get; set; }

        bool CanMove(List<ILocateable> map, Direction direction);
        void Move(List<ILocateable> map, Direction direction);
    }
}