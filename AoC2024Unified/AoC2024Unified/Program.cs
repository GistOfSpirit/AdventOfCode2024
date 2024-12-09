﻿using AoC2024Unified.Solutions;

var daySolutions = new Type[]{
    typeof(Day1Solution),
    typeof(Day2Solution),
    typeof(Day3Solution),
    typeof(Day4Solution),
    typeof(Day5Solution),
    typeof(Day6Solution),
    typeof(Day7Solution),
    typeof(Day8Solution),
    typeof(Day9Solution),
};

int dayNum = 8;
bool isReal = false;

var daySolution = (IDaySolution)Activator.CreateInstance(
    daySolutions[dayNum - 1])!;

await daySolution.Solve(isReal);