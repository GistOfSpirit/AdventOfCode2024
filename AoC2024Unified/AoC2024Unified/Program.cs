using AoC2024Unified.Solutions;

var daySolutions = new IDaySolution[]{
    new Day1Solution(),
    new Day2Solution(),
    new Day3Solution(),
    new Day4Solution(),
    new Day5Solution(),
    new Day6Solution(),
    new Day7Solution()
};

int dayNum = 7;
bool isReal = false;

await daySolutions[dayNum - 1].Solve(isReal);