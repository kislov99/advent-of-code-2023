global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests11_2
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
#...#.....", ExpectedResult = 8410)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return new Task11_2().ProcessLines(lines);
        }
    }
}