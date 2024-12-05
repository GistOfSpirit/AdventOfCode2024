namespace AoC2024Unified.Solutions
{
    public class Day4Solution : IDaySolution
    {
        public async Task Solve(bool isReal)
        {
            var solA = new Day4ASolution();
            var solB = new Day4BSolution();

            await solA.Solve(isReal);
            await solB.Solve(isReal);
        }
    }
}