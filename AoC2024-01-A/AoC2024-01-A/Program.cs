string inputFilePath = Path.Combine(AppContext.BaseDirectory, "input.txt");
string inputFileContents = await File.ReadAllTextAsync(inputFilePath);

var leftList = new List<int>();
var rightList = new List<int>();

foreach (string inputRow in inputFileContents.Split(
    Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
{
    string[] strArray
        = inputRow.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    leftList.Add(Convert.ToInt32(strArray[0]));
    rightList.Add(Convert.ToInt32(strArray[1]));
}

if (leftList.Count != rightList.Count)
{
    Console.WriteLine("Invalid list count");
    return;
}

leftList.Sort();
rightList.Sort();

int totalDistance = 0;
int similarityScore = 0;

for (int i = 0; i < leftList.Count; ++i)
{
    int leftNum = leftList[i];
    int rightNum = rightList[i];

    int distance = Math.Abs(leftNum - rightNum);

    totalDistance += distance;

    int rightCount = rightList.Count((n) => n == leftNum);
    int similarity = leftNum * rightCount;

    similarityScore += similarity;
}

Console.WriteLine($"Total distance is: {totalDistance}");
Console.WriteLine($"Similarity is: {similarityScore}");