global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests7_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"4AA4A 1
AAKKK 1
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
99488 1
286TT 1", ExpectedResult = 9292)]
        public long Test(string input)
        {
            var lines = input.Split("\r\n");
            return Task7_1.ProcessLines(lines);
        }
    }
}