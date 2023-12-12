global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests9_2
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45", ExpectedResult = 2)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return Task9_2.ProcessLines(lines);
            return Task9_2.ProcessLines(lines);
        }
    }
}