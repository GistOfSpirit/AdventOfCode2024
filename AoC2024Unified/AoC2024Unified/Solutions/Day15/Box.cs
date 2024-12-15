namespace AoC2024Unified.Solutions.Day15
{
    public class Box : Movable
    {
        private const int YCoordFactor = 100;

        public int Coordinate => (Location.Y * YCoordFactor) + Location.X;
    }
}