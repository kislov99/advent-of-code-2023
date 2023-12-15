global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests13_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#", ExpectedResult = 405)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return new Task13_1().ProcessLines(lines);
        }
    }
}