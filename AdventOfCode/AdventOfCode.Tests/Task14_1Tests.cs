global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests14_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....", ExpectedResult = 136)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return new Task14_1().ProcessLines(lines);
        }
    }
}