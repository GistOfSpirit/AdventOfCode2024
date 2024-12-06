using AoC2024Unified.Solutions;

var daySolutions = new IDaySolution[]{
    new Day1Solution(),
    new Day2Solution(),
    new Day3Solution(),
    new Day4Solution(),
    new Day5Solution(),
    new Day6Solution()
};

int dayNum = 6;
bool isReal = false;

await daySolutions[dayNum - 1].Solve(isReal);
