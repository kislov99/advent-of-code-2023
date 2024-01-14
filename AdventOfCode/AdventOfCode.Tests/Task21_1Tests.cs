global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests21_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........", ExpectedResult = 42)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return new Task21_1().ProcessLines(lines);
        }
    }
}