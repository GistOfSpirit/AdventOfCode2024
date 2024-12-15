using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public abstract class Movable : ILocateable
    {
        public Point Location { get; set; }

        public virtual bool CanMove(List<ILocateable> map, Direction direction)
        {
            Point wantedLocation = Directions.GetNextPoint(direction, Location);

            ILocateable? objectAhead
                = map.FirstOrDefault((l) => l.Location == wantedLocation);

            return (
                objectAhead == null
                || objectAhead.CanMove(map, direction)
            );
        }

        public virtual void Move(List<ILocateable> map, Direction direction)
        {
            Point wantedLocation = Directions.GetNextPoint(direction, Location);

            ILocateable? objectAhead
                = map.FirstOrDefault((l) => l.Location == wantedLocation);

            if (
                objectAhead == null
                || objectAhead.CanMove(map, direction)
            )
            {
                objectAhead?.Move(map, direction);
                Location = wantedLocation;
            }
        }
    }
}