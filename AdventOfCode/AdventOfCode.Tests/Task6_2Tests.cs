global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests6_2
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"Time:      7  15   30
Distance:  9  40  200", ExpectedResult = 71503)]
        public long Test(string input)
        {
            var lines = input.Split("\r\n");
            return Task6_2.ProcessLines(lines);
        }
    }
}