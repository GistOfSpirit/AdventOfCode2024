namespace AoC2024Unified.Types
{
    public class Day5Input
    {
        public required NumberRowList OrderingRules { init; get; }
        public required NumberRowList PageUpdates { init; get; }

        public static Day5Input Parse(string input)
        {
            var orderingRules = new NumberRowList();
            var pageUpdates = new NumberRowList();

            foreach (string inputRow in input.Split(
                Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                if (inputRow.Contains('|'))
                {
                    string[] strArray = inputRow.Split("|",
                        StringSplitOptions.RemoveEmptyEntries);

                    List<int> intList = strArray.Select(
                        (s) => Convert.ToInt32(s)).ToList();

                    orderingRules.Add(intList);
                }
                else
                {
                    string[] strArray = inputRow.Split(",",
                        StringSplitOptions.RemoveEmptyEntries);

                    List<int> intList = strArray.Select(
                        (s) => Convert.ToInt32(s)).ToList();

                    pageUpdates.Add(intList);
                }
            }

            return new Day5Input
            {
                OrderingRules = orderingRules,
                PageUpdates = pageUpdates
            };
        }
    }
}