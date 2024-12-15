using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public class BoxWest : BoxMain
    {
        private BoxEast GetBoxEast(List<ILocateable> map)
        {
            ILocateable? objectEast = map.FirstOrDefault(
                (l) => l.Location.X == Location.X + 1
                && l.Location.Y == Location.Y);

            if (
                objectEast != null
                && objectEast is BoxEast boxEast
                )
            {
                return boxEast;
            }

            throw new InvalidOperationException("Invalid box pair placement");
        }

        public override bool CanMove(List<ILocateable> map, Direction direction)
        {
            return CanMove(map, direction, false);
        }

        public bool CanMove(List<ILocateable> map, Direction direction,
            bool checkedOther)
        {
            if (direction == Direction.East
                || direction == Direction.West
                || checkedOther)
            {
                return base.CanMove(map, direction);
            }
            else
            {
                return base.CanMove(map, direction)
                    && GetBoxEast(map).CanMove(map, direction, true);
            }
        }

        public override void Move(List<ILocateable> map, Direction direction)
        {
            Move(map, direction, false);
        }

        public void Move(List<ILocateable> map, Direction direction,
            bool movedOther)
        {
            if (direction == Direction.East
                || direction == Direction.West
                || movedOther)
            {
                base.Move(map, direction);
            }
            else
            {
                GetBoxEast(map).Move(map, direction, true);
                base.Move(map, direction);
            }
        }
    }
}