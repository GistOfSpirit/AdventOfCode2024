namespace AoC2024Unified.Extensions
{
    public static class NumberExtensions
    {
        public static ulong GetNumberOfDigits(this ulong num)
        {
            if (num == 0)
            {
                return 1;
            }

            return (ulong)Math.Floor(Math.Log10(num) + 1);
        }
    }
}