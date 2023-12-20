global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests16_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....", ExpectedResult = 46)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return new Task16_1().ProcessLines(lines);
        }
    }
}