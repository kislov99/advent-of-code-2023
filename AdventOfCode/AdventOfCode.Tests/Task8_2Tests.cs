global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests8_2
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)", ExpectedResult = 6)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return Task8_2.ProcessLines(lines);
        }
    }
}