using AoC2024Unified.Solutions;

var daySolutions = new IDaySolution[]{
    new Day1Solution()
};

int dayNum = 1;
bool isReal = false;

await daySolutions[dayNum - 1].Solve(isReal);