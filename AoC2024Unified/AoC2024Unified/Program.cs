using AoC2024Unified.Solutions;

var daySolutions = new IDaySolution[]{
    new Day1Solution(),
    new Day2Solution(),
    new Day3Solution(),
    new Day4Solution()
};

int dayNum = 4;
bool isReal = false;

await daySolutions[dayNum - 1].Solve(isReal);
