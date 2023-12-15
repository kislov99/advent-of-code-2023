global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests13_2
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
#....#..#", ExpectedResult = 400)]
        public long Test1(string input)
        {
            var lines = input.Split("\r\n");
            return new Task13_2().ProcessLines(lines);
        }
    }
}