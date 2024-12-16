using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public class Box : Movable
    {
        private const int YCoordFactor = 100;

        public Box? PairedBox { get; set; } = null;
        public bool DisableDetection { get; set; } = false;
        public int Coordinate => DisableDetection
            ? 0
            : (Location.Y * YCoordFactor) + Location.X;

        public override bool CanMove(List<ILocateable> map, Direction direction)
        {
            return CanMove(map, direction, false);
        }

        public bool CanMove(List<ILocateable> map, Direction direction,
            bool checkedPaired)
        {
            if (PairedBox == null
                || direction == Direction.East
                || direction == Direction.West
                || checkedPaired)
            {
                return base.CanMove(map, direction);
            }
            else
            {
                return base.CanMove(map, direction)
                    && PairedBox.CanMove(map, direction, true);
            }
        }

        public override void Move(List<ILocateable> map, Direction direction)
        {
            Move(map, direction, false);
        }

        public void Move(List<ILocateable> map, Direction direction,
            bool movedOther)
        {
            if (PairedBox == null
                || direction == Direction.East
                || direction == Direction.West
                || movedOther)
            {
                base.Move(map, direction);
            }
            else
            {
                PairedBox.Move(map, direction, true);
                base.Move(map, direction);
            }
        }
    }
}