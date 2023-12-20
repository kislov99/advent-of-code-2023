global using NUnit.Framework;

namespace AdventOfCode.Tests
{
    public class Tests17_1
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(
@"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533", ExpectedResult = 94)]
        public long Test2(string input)
        {
            var lines = input.Split("\r\n");
            return new Task17().ProcessLines(lines);
        }
    }
}