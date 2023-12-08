global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests7_2
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
JKKK2 1
QQQQ2 1", ExpectedResult = 5905)]
        public long Test(string input)
        {
            var lines = input.Split("\r\n");
            return Task7_2.ProcessLines(lines);
        }
    }
}