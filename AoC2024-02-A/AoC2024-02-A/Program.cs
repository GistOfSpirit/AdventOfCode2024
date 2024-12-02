bool onlyIncreasing(IList<int> intList)
{
    for (int i = 0; i < intList.Count - 1; ++i)
    {
        if (intList[i + 1] < intList[i])
        {
            return false;
        }
    }

    return true;
}

bool onlyDecreasing(IList<int> intList)
{
    for (int i = 0; i < intList.Count - 1; ++i)
    {
        if (intList[i + 1] > intList[i])
        {
            return false;
        }
    }

    return true;
}

int getMaxDiff(IList<int> intList)
{
    int maxDiff = 0;

    for (int i = 0; i < intList.Count - 1; ++i)
    {
        maxDiff = Math.Max(maxDiff, Math.Abs(intList[i + 1] - intList[i]));
    }

    return maxDiff;
}

int getMinDiff(IList<int> intList)
{
    int minDiff = Math.Abs(intList[1] - intList[0]);

    for (int i = 1; i < intList.Count - 1; ++i)
    {
        minDiff = Math.Min(minDiff, Math.Abs(intList[i + 1] - intList[i]));
    }

    return minDiff;
}

bool isRowSafe(IList<int> intList)
{
    return (
            onlyIncreasing(intList)
            || onlyDecreasing(intList)
        )
        && (getMinDiff(intList) >= 1)
        && (getMaxDiff(intList) <= 3);
}

IList<IList<int>> getProblemDampenerOptions(IList<int> intList)
{
    var options = new List<IList<int>>();

    for (int ignore = 0; ignore < intList.Count; ++ignore)
    {
        var newList = new List<int>(intList);

        newList.RemoveAt(ignore);

        options.Add(newList);
    }

    return options;
}

string inputFilePath = Path.Combine(AppContext.BaseDirectory, "input.txt");
string inputFileContents = await File.ReadAllTextAsync(inputFilePath);

var rowList = new List<IList<int>>();

foreach (string inputRow in inputFileContents.Split(
    Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
{
    string[] strArray
        = inputRow.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    IList<int> intList = strArray.Select((s) => Convert.ToInt32(s)).ToList();

    rowList.Add(intList);
}

int safeRowCount = 0;

foreach (IList<int> intList in rowList)
{
    if (isRowSafe(intList))
    {
        safeRowCount++;
    }
    else
    {
        IList<IList<int>> pdOptions = getProblemDampenerOptions(intList);

        foreach (IList<int> pdList in pdOptions)
        {
            if (isRowSafe(pdList))
            {
                safeRowCount++;
                break;
            }
        }
    }
}

Console.WriteLine($"There are {safeRowCount} safe rows.");