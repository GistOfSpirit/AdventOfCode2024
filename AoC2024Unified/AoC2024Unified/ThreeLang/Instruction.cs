namespace AoC2024Unified.ThreeLang
{
    public class Instruction
    {
        public required string Name { init; get; }

        public required Type[] Operands { init; get; }

        public required Type? ReturnType { init; get; }

        public bool AlwaysEnabled { init; get; } = false;

        public required Func<object[], object> Action { init; get; }
    }
}