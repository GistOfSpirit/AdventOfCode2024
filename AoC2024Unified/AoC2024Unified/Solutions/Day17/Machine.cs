namespace AoC2024Unified.Solutions.Day17
{
    public class Machine(int regA, int regB, int regC, List<int> program)
    {
        private const int InstrJump = 2;

        private int RegisterA { get; set; } = regA;
        private int RegisterB { get; set; } = regB;
        private int RegisterC { get; set; } = regC;
        private List<int> Program { init; get; } = program;

        private int InstrPointer { get; set; } = 0;
        private int? NextInstr { get; set; } = null;
        private List<int> Output { get; set; } = [];

        private int GetComboValue(int value)
            => value switch
            {
                >= 0 and <= 3 => value,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                _ => throw new InvalidOperationException(
                    $"Invalid combo value: {value}")
            };

        private int Dv(int operand)
            => (int)(RegisterA / Math.Pow(2, GetComboValue(operand)));
        private void Adv(int operand) => RegisterA = Dv(operand);
        private void Bdv(int operand) => RegisterB = Dv(operand);
        private void Cdv(int operand) => RegisterC = Dv(operand);
        private void Bxl(int operand) => RegisterB ^= operand;
        private void Bst(int operand) => RegisterB = GetComboValue(operand) % 8;
        private void Jnz(int operand)
            => NextInstr = RegisterA switch
            {
                0 => NextInstr,
                _ => operand
            };
        private void Bxc(int _) => RegisterB ^= RegisterC;
        private void Out(int operand) => Output.Add(GetComboValue(operand) % 8);

        private Action<int> GetInstruction(int opcode)
            => opcode switch
            {
                0 => Adv,
                1 => Bxl,
                2 => Bst,
                3 => Jnz,
                4 => Bxc,
                5 => Out,
                6 => Bdv,
                7 => Cdv,
                _ => throw new InvalidOperationException(
                    $"Invalid opcode: {opcode}")
            };

        private void DoInstruction(int opcode, int operand)
        {
            var instr = GetInstruction(opcode);
            instr(operand);
            InstrPointer = NextInstr ?? (InstrPointer += InstrJump);
            NextInstr = null;
        }

        public List<int> Run()
        {
            while (InstrPointer < Program.Count)
            {
                DoInstruction(Program[InstrPointer], Program[InstrPointer + 1]);
            }

            return Output;
        }
    }
}