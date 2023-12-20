global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests15_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("HASH", ExpectedResult = 52)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return new Task15_1().ProcessLines(lines);
        }

        [TestCase(@"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7", ExpectedResult = 145)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return new Task15_1().ProcessLines(lines);
        }
    }
}