global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests12_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1", ExpectedResult = 21)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return new Task12_1().ProcessLines(lines);
        }
    }
}