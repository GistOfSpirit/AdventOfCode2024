namespace AoC2024Unified.Solutions
{
    public interface IDaySolutionWithInput : IDaySolution
    {
        Task Solve(bool isReal, bool skipIfRequiresInput);
    }
}