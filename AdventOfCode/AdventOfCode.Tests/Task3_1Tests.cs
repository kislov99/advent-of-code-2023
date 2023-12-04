global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests3_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"
467....114
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
", ExpectedResult = 4361)]
        public int Test(string input)
        {
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            return Task3_1.ProcessLines(lines);
        }
    }
}