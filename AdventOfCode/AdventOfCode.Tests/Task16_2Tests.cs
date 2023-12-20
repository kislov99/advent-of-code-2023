global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests16_2
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
..//.|....", ExpectedResult = 51)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return new Task16_2().ProcessLines(lines);
        }
    }
}