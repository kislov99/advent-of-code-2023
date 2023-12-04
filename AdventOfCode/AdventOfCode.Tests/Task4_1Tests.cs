global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests4_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53", ExpectedResult = 8)]
        [TestCase("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19", ExpectedResult = 2)]
        [TestCase("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1", ExpectedResult = 2)]
        [TestCase("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83", ExpectedResult = 1)]
        [TestCase("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36", ExpectedResult = 0)]
        [TestCase("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", ExpectedResult = 0)]
        
        public int Test(string input)
        {
            return Task4_1.ProcessLine(input);
        }
    }
}