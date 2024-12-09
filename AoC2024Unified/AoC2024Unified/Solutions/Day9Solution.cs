namespace AoC2024Unified.Solutions
{
    public class Day9Solution : IDaySolution
    {
        private class FileBlock
        {
            public int FileId { get; set; }

            public int Size { get; set; }

            public int SpaceAfter { get; set; }

            public override string ToString()
                => $"{FileId} ({Size}) ({SpaceAfter})";
        }

        private const int DayNum = 9;

        private static List<FileBlock> TranslateInput(string input)
        {
            var list = new List<FileBlock>();

            for (int i = 0; i < input.Length; i += 2)
            {
                var block = new FileBlock
                {
                    FileId = i / 2,
                    Size = int.Parse(input[i].ToString())
                };

                if (i + 1 < input.Length)
                {
                    block.SpaceAfter = int.Parse(input[i + 1].ToString());
                }

                list.Add(block);
            }

            return list;
        }

        private static bool IsGathered(List<FileBlock> fileBlockList)
            => fileBlockList[..^1].All((b) => b.SpaceAfter == 0);

        private static void EnfragDisk(List<FileBlock> fileBlockList)
        {
            do
            {
                FileBlock firstWithBlank = fileBlockList
                    .First((b) => b.SpaceAfter > 0);

                FileBlock lastBlock = fileBlockList[^1];

                int numToMove = Math.Min(
                    firstWithBlank.SpaceAfter, lastBlock.Size);

                FileBlock targetBlock;

                if (firstWithBlank.FileId == lastBlock.FileId)
                {
                    // We just need to grow the block
                    targetBlock = firstWithBlank;
                }
                else
                {
                    FileBlock blockAfterBlank = fileBlockList[
                        fileBlockList.IndexOf(firstWithBlank) + 1];

                    if (blockAfterBlank.FileId == lastBlock.FileId)
                    {
                        targetBlock = blockAfterBlank;
                        targetBlock.SpaceAfter += firstWithBlank.SpaceAfter;
                    }
                    else
                    {
                        targetBlock = new FileBlock
                        {
                            FileId = lastBlock.FileId,
                            Size = 0,
                            SpaceAfter = firstWithBlank.SpaceAfter
                        };

                        fileBlockList.Insert(
                            fileBlockList.IndexOf(firstWithBlank) + 1,
                            targetBlock
                        );
                    }

                    firstWithBlank.SpaceAfter = 0;
                }

                targetBlock.Size += numToMove;
                targetBlock.SpaceAfter -= numToMove;

                lastBlock.Size -= numToMove;
                lastBlock.SpaceAfter += numToMove;

                if (lastBlock.Size == 0)
                {
                    fileBlockList.Remove(lastBlock);
                    fileBlockList[^1].SpaceAfter += lastBlock.SpaceAfter;
                }
            } while (!IsGathered(fileBlockList));
        }

        private static ulong CalcChecksum(List<FileBlock> fileBlockList)
        {
            ulong checksum = 0;
            int index = 0;

            checked
            {
                foreach (FileBlock block in fileBlockList)
                {
                    for (int i = 0; i < block.Size; ++i)
                    {
                        checksum += (ulong)block.FileId * (ulong)(i + index);
                    }

                    index += block.Size;
                }
            }

            return checksum;
        }

        public async Task Solve(bool isReal)
        {
            string input = (await Common.ReadFile(isReal, DayNum)).Trim();

            var fileBlockList = TranslateInput(input);
            EnfragDisk(fileBlockList);

            ulong checksum = CalcChecksum(fileBlockList);

            Console.WriteLine($"The checksum is {checksum}");
        }
    }
}