using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public abstract class Movable : ILocateable
    {
        public Point Location { get; set; }

        public bool Move(List<ILocateable> map, Direction direction)
        {
            Point wantedLocation = Directions.GetNextPoint(direction, Location);

            ILocateable? objectAhead
                = map.FirstOrDefault((l) => l.Location == wantedLocation);

            if (
                objectAhead == null
                || (
                    objectAhead is Movable movable
                    && movable.Move(map, direction)
                )
            )
            {
                Location = wantedLocation;
                return true;
            }

            return false;
        }
    }
}