using System.Text.RegularExpressions;

namespace AoC2024Unified.ThreeLang
{
    public class ThreeLangParser(string inputCode)
    {
        private InstructionSet instructionSet = new();

        private string InputCode { init; get; } = inputCode;

        private int FindNextInstructionIndex(int startIndex)
            => InputCode.IndexOf('(', startIndex);

        private Instruction? GetInstructionAt(int index)
        {
            if (InputCode[index] != '(')
            {
                return null;
            }

            foreach (Instruction instr in instructionSet.Instructions)
            {
                int startIndex = index - instr.Name.Length;

                if (InputCode[startIndex..index] == instr.Name)
                {
                    return instr;
                }
            }

            return null;
        }

        private object[] GetOperands(int index, Instruction instr)
        {
            string pattern = @"([^,]+),?";

            int startIndex = index + 1;
            int endIndex = InputCode.IndexOf(')', startIndex);

            MatchCollection matches =
                Regex.Matches(InputCode[startIndex..endIndex], pattern,
                RegexOptions.None, TimeSpan.FromSeconds(3))
                ?? throw new InvalidOperationException("Null object matches");

            if (instr.Operands.Length != matches.Count)
            {
                throw new InvalidOperationException(
                    "Invalid number of operands");
            }

            var operands = new object[matches.Count];

            for (int i = 0; i < matches.Count; ++i)
            {
                try
                {
                    operands[i] = Convert.ChangeType(
                        matches[i].Groups[1].Value, instr.Operands[i]);
                }
                catch (FormatException)
                {
                    throw new InvalidOperationException(
                        "Invalid type of operands");
                }
            }

            return operands;
        }

        private static object? ExecuteInstruction(
            Instruction instr, object[] operands)
        {
            if (operands.Length != instr.Operands.Length)
            {
                throw new InvalidOperationException(
                    "Invalid number of operands");
            }

            return instr.Action(operands);
        }

        public int ParseCode()
        {
            int total = 0;
            int currentIndex = FindNextInstructionIndex(0);

            while (currentIndex >= 0)
            {
                Instruction? instr = GetInstructionAt(currentIndex);

                if (
                    instr == null
                    || (
                        !instructionSet.Enabled
                        && !instr.AlwaysEnabled
                    )
                )
                {
                    currentIndex = FindNextInstructionIndex(currentIndex + 1);
                    continue;
                }

                try
                {
                    object[] operands = GetOperands(currentIndex, instr);

                    object? result = ExecuteInstruction(instr, operands);

                    if (result != null && result.GetType() == typeof(int))
                    {
                        total += (int)result;
                    }
                }
                catch (InvalidOperationException)
                {
                    currentIndex = FindNextInstructionIndex(currentIndex + 1);
                    continue;
                }

                currentIndex = FindNextInstructionIndex(currentIndex + 1);
            }

            return total;
        }
    }
}