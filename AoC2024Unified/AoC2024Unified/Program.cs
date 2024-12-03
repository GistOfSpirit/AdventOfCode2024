using AoC2024Unified.Solutions;

var daySolutions = new IDaySolution[]{
    new Day1Solution(),
    new Day2Solution()
};

int dayNum = 2;
bool isReal = false;

await daySolutions[dayNum - 1].Solve(isReal);