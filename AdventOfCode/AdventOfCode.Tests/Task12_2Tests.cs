global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests12_2
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
?###???????? 3,2,1", ExpectedResult = 525152)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return new Task12_2().ProcessLines(lines);
        }
    }
}