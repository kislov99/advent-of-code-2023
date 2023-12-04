global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests2_2
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", ExpectedResult = 48)]
        [TestCase("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", ExpectedResult = 12)]
        [TestCase("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", ExpectedResult = 1560)]
        [TestCase("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", ExpectedResult = 630)]
        [TestCase("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", ExpectedResult = 36)]
        public int Test(string input)
        {
           return Task2_2.ProcessLine(input);
        }
    }
}