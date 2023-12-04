global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("2xjzgsjzfhzhm1", ExpectedResult = 21)]
        [TestCase("qhklfjd39rpjxhqtftwopfvrrj2eight", ExpectedResult = 38)]
        [TestCase("95btwo", ExpectedResult = 92)]
        [TestCase("lfsqldnf3onenplgfxdjzjjnpzfxnineseven", ExpectedResult = 37)]
        [TestCase("five7fouronesevenpfsrmszpc", ExpectedResult = 57)]
        [TestCase("two1nine", ExpectedResult = 29)]
        [TestCase("eightwothree", ExpectedResult = 83)]
        [TestCase("abcone2threexyz", ExpectedResult = 13)]
        [TestCase("xtwone3four", ExpectedResult = 24)]
        [TestCase("4nineeightseven2", ExpectedResult = 42)]
        [TestCase("zoneight234", ExpectedResult = 14)]
        [TestCase("7pqrstsixteen", ExpectedResult = 76)]
        public int TestForTask1(string input)
        {
           return Task1.ProcessLine(input);
        }
    }
}