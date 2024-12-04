using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public static class Common
    {
        public static async Task<string> ReadFile(string day, bool isReal)
        {
            string fileName = $"{(isReal ? "real" : "test")}{day}.txt";
            string inputFilePath = Path.Combine(
                AppContext.BaseDirectory, "inputs", fileName);
            string inputFileContents
                = await File.ReadAllTextAsync(inputFilePath);
            return inputFileContents;
        }

        public static async Task<NumberRowList> ReadNumberRows(
            string day, bool isReal)
        {
            string inputFileContents = await ReadFile(day, isReal);

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
            string day, bool isReal)
        {
            string fileContents = await ReadFile(day, isReal);

            string[] rows = fileContents.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

            return rows;
        }
    }
}