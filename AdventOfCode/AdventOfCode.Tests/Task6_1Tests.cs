global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests6_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"Time:      7  15   30
Distance:  9  40  200", ExpectedResult = 288)]
        public long Test(string input)
        {
            var lines = input.Split("\r\n");
            return Task6_1.ProcessLines(lines);
        }
    }
}