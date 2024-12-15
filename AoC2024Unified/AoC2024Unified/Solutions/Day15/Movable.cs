using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions.Day15
{
    public abstract class Movable : ILocateable
    {
        public Point Location { get; set; }
        public int Width { get; set; }

        private List<ILocateable> GetObjectsAhead(List<ILocateable> map,
            Direction direction)
        {
            Point wantedLocation = direction.GetNextPoint(Location);

            if (direction == Direction.East || direction == Direction.West)
            {
                return [map.FirstOrDefault(
                    (l) => l.Location == wantedLocation)];
            }
            else
            {
                return Enumerable.Range(0, Width)
                    .Select((y) => map.FirstOrDefault(
                        (l) => l.Location == wantedLocation + new Size(0, y)))
                    .Where((l) => l != null)
                    .Cast<ILocateable>()
                    .ToList();
            }
        }

        private bool TryMove(List<ILocateable> map, Direction direction,
            bool doMove)
        {
            Point wantedLocation = direction.GetNextPoint(Location);

            List<ILocateable> objectsAhead = GetObjectsAhead(map, direction);

            if (
                objectsAhead.Count == 0
                || objectsAhead.All((l) => l.CanMove(map, direction))
            )
            {
                if (doMove)
                {
                    objectsAhead.ForEach((l) => l.Move(map, direction));
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