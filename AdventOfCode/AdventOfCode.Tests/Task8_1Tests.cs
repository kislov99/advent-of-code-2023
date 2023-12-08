global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests8_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)", ExpectedResult = 2)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return Task8_1.ProcessLines(lines);
        }

        [TestCase(
@"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)", ExpectedResult = 6)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return Task8_1.ProcessLines(lines);
        }
    }
}