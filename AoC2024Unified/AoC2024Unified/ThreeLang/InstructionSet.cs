namespace AoC2024Unified.ThreeLang
{
    public class InstructionSet
    {
        public Instruction[] Instructions { get; }

        public bool Enabled { get; private set; } = true;

        public InstructionSet()
        {
            var mulInstr = new Instruction
            {
                Name = "mul",
                Operands = [typeof(int), typeof(int)],
                ReturnType = typeof(int),
                Action = (input) => (int)input[0] * (int)input[1]
            };

            var doInstr = new Instruction
            {
                Name = "do",
                Operands = [],
                ReturnType = null,
                AlwaysEnabled = true,
                Action = (_) => Enabled = true
            };

            var dontInstr = new Instruction
            {
                Name = "don't",
                Operands = [],
                ReturnType = null,
                Action = (_) => Enabled = false
            };

            Instructions =
            [
                mulInstr,
                doInstr,
                dontInstr
            ];
        }
    }
}