using AoC2024Unified.Types;

namespace AoC2024Unified.Solutions
{
    public static class Common
    {
        public static async Task<string> ReadFile(int day, bool isReal)
        {
            string fileName = $"{(isReal ? "real" : "test")}{day}.txt";
            string inputFilePath = Path.Combine(
                AppContext.BaseDirectory, "inputs", fileName);
            string inputFileContents
                = await File.ReadAllTextAsync(inputFilePath);
            return inputFileContents;
        }

        public static async Task<NumberRowList> ReadNumberRows(
            int day, bool isReal)
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
    }
}