global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests18_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 10 (#015232)
D 4 (#015232)
R 2 (#015232)
U 2 (#7a21e3)
R 1 (#7a21e3)
D 3 (#7a21e3)
L 10 (#7a21e3)
U 3 (#7a21e3)
R 1 (#7a21e3)
D 1 (#7a21e3)
R 4 (#7a21e3)
U 5 (#7a21e3)
R 11 (#7a21e3)", ExpectedResult = 62)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return new Task18_1().ProcessLines(lines);
        }
    }
}