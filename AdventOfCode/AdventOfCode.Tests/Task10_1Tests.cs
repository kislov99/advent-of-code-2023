global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests10_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"..F7.
.FJ|.
SJ.L7
|F--J
LJ...", ExpectedResult = 8)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return Task10_1.ProcessLines(lines);
        }
    }
}