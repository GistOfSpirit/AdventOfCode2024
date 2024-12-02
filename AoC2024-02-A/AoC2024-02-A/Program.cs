bool onlyIncreasing(int[] intArray)
{
    for (int i = 0; i < intArray.Length - 1; ++i)
    {
        if (intArray[i + 1] < intArray[i])
        {
            return false;
        }
    }

    return true;
}

bool onlyDecreasing(int[] intArray)
{
    for (int i = 0; i < intArray.Length - 1; ++i)
    {
        if (intArray[i + 1] > intArray[i])
        {
            return false;
        }
    }

    return true;
}

int getMaxDiff(int[] intArray)
{
    int maxDiff = 0;

    for (int i = 0; i < intArray.Length - 1; ++i)
    {
        maxDiff = Math.Max(maxDiff, Math.Abs(intArray[i + 1] - intArray[i]));
    }

    return maxDiff;
}

int getMinDiff(int[] intArray)
{
    int minDiff = Math.Abs(intArray[1] - intArray[0]);

    for (int i = 1; i < intArray.Length - 1; ++i)
    {
        minDiff = Math.Min(minDiff, Math.Abs(intArray[i + 1] - intArray[i]));
    }

    return minDiff;
}

bool isRowSafe(int[] intArray)
{
    return (
            onlyIncreasing(intArray)
            || onlyDecreasing(intArray)
        )
        && (getMinDiff(intArray) >= 1)
        && (getMaxDiff(intArray) <= 3);
}

string inputFilePath = Path.Combine(AppContext.BaseDirectory, "input.txt");
string inputFileContents = await File.ReadAllTextAsync(inputFilePath);

var rowList = new List<int[]>();

foreach (string inputRow in inputFileContents.Split(
    Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
{
    string[] strArray
        = inputRow.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    int[] intArray = strArray.Select((s) => Convert.ToInt32(s)).ToArray();

    rowList.Add(intArray);
}

int safeRowCount = 0;

foreach (int[] intArray in rowList)
{
    if (isRowSafe(intArray))
    {
        safeRowCount++;
    }
}

Console.WriteLine($"There are {safeRowCount} safe rows.");