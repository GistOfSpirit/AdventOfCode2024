using AoC2024Unified.Solutions;

int dayNum = 15;
bool isReal = false;

Type? daySolutionType = Type.GetType(
    $"AoC2024Unified.Solutions.Day{dayNum}Solution");

if (daySolutionType == null)
{
    Console.WriteLine($"Could not find solution for day {dayNum}");
    return;
}

var daySolution = (IDaySolution)Activator.CreateInstance(daySolutionType)!;

if (
    args.Length > 0
    && args[0] == "input"
    && daySolution is IDaySolutionWithInput daySolutionWithInput
)
{
    await daySolutionWithInput.Solve(isReal, false);
}
else
{
    await daySolution.Solve(isReal);
}
