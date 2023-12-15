global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests11_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....", ExpectedResult = 374)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return new Task11_1().ProcessLines(lines);
        }
    }
}