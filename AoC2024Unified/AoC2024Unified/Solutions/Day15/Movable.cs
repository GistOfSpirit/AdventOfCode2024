using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public abstract class Movable : ILocateable
    {
        public Point Location { get; set; }

        private bool TryMove(List<ILocateable> map, Direction direction,
            bool doMove)
        {
            Point wantedLocation = Directions.GetNextPoint(direction, Location);

            ILocateable? objectAhead
                = map.FirstOrDefault((l) => l.Location == wantedLocation);

            if (
                objectAhead == null
                || objectAhead.CanMove(map, direction)
            )
            {
                if (doMove)
                {
                    objectAhead?.Move(map, direction);
                    Location = wantedLocation;
                }

                return true;
            }

            return false;
        }

        public bool CanMove(List<ILocateable> map, Direction direction)
            => TryMove(map, direction, false);

        public void Move(List<ILocateable> map, Direction direction)
            => TryMove(map, direction, true);
    }
}