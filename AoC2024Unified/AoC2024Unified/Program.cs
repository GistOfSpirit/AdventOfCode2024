using AoC2024Unified.Solutions;

var daySolutions = new IDaySolution[]{
    new Day1Solution(),
    new Day2Solution(),
    new Day3Solution(),
    new Day4Solution(),
    new Day5Solution()
};

int dayNum = 5;
bool isReal = true;

await daySolutions[dayNum - 1].Solve(isReal);
