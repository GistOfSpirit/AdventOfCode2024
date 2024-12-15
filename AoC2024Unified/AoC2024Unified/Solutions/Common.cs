using System.Drawing;
using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public static class Common
    {
        public static async Task<string> ReadFile(bool isReal, int day,
            string sub = "")
        {
            string fileName = $"{(isReal ? "real" : "test")}{day}{sub}.txt";
            string inputFilePath = Path.Combine(
                AppContext.BaseDirectory, "inputs", fileName);
            string inputFileContents
                = await File.ReadAllTextAsync(inputFilePath);
            return inputFileContents;
        }

        public static async Task<NumberRowList> ReadNumberRows(
            bool isReal, int day, string sub = "")
        {
            string inputFileContents = await ReadFile(isReal, day, sub);

            var rowList = new NumberRowList();

            foreach (string inputRow in inputFileContents.Split(
                Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] strArray = inputRow.Split(" ",
                    StringSplitOptions.RemoveEmptyEntries);

                List<int> intList = strArray.Select(
                    (s) => Convert.ToInt32(s)).ToList();

                rowList.Add(intList);
            }

            return rowList;
        }

        public static async Task<string[]> ReadFileAsMatrix(
            bool isReal, int day, string sub = "")
        {
            string fileContents = await ReadFile(isReal, day, sub);

            return ConvertToMatrix(fileContents);
        }

        public static string[] ConvertToMatrix(string fileContents)
            => fileContents.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

        public static void UpdateMatrix(
            string[] matrix, Point loc, char newChar)
        {
            string row = matrix[loc.Y];
            string newRow = row.Remove(loc.X, 1)
                .Insert(loc.X, newChar.ToString());
            matrix[loc.Y] = newRow;
        }
    }
}