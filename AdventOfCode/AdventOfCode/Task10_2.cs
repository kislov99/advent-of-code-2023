using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task10_2
{
    class Point
    {
        public Coord Coord;
        public bool Visited = false;
        public long Number;
    }

    class Pipe
    {
        public Coord Coord;
        public long Number;
        public Pipe Next;
        public Direction From;
    }

    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task10.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static char[,] Matrix;
    static Point[,] resultMatrix;
    static Pipe Pipeline;

    public static long ProcessLines(string[] lines)
    {
        Coord start = new Coord();
        Matrix = new char[lines.Length, lines[0].Length];
        resultMatrix = new Point[lines.Length, lines[0].Length];
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                if (c == 'S')
                {
                    start.X = x;
                    start.Y = y;
                }
                Matrix[y, x] = c;
                resultMatrix[y, x] = new Point() { Number = 0 };
            }
        }
        {
            var x = start.X + 1;
            var y = start.Y;
            Direction from = Direction.West;
            long number = 0;
            Pipeline = new Pipe()
            {
                Number = -3,
                From = Direction.South,
                Coord = new Coord() { X = start.X, Y = start.Y },
            };

            var pipe = Pipeline;
            resultMatrix[start.Y, start.X].Number = -3;
            while (Matrix[y, x] != 'S')
            {
                pipe.Next = new Pipe()
                {
                    Coord = new Coord() { X = x, Y = y },
                    Number = number + 1,
                    From = from
                };
                pipe = pipe.Next;
                (from, x, y, number) = EnumeratePipeLine(from, x, y, number);
            }
        }

        PrintMatrix("matrix1.txt");
        MarkOutSideClasters();
        PrintMatrix("matrix2.txt");
        MarkSqeezing();
        PrintMatrix("matrix3.txt");
        MarkInsideClasters();
        PrintMatrix("matrix4.txt");

        var result = 0;
        for (var y = 0; y < resultMatrix.GetLength(0); y++)
        {
            for (var x = 0; x < resultMatrix.GetLength(1); x++)
            {
                if (resultMatrix[y, x].Number == -2)
                    result++;
            }
        }

        return result;
    }

    static long MaxNumber;

    static void PrintMatrix(string filename)
    {
        File.Delete(filename);
        for (var y = 0; y < resultMatrix.GetLength(0); y++)
        {
            for (var x = 0; x < resultMatrix.GetLength(1); x++)
            {
                //File.AppendAllText(filename, (resultMatrix[y, x].Number % 100).ToString().PadLeft(2, '0') + " ");
                File.AppendAllText(filename, (resultMatrix[y, x].Number % 100000).ToString().PadLeft(5, '0') + " ");
            }
            File.AppendAllText(filename, Environment.NewLine);
        }
    }


    static void MarkSqeezing()
    {
        var pipe = Pipeline.Next;
        var found = false;
        bool rightOutside = false;

        while (pipe.Next != null && !found)
        {
            if (pipe.From == Direction.North)
            {
                var checkX = pipe.Coord.X - 1;
                var checkY = pipe.Coord.Y;
                if (checkX < 0 || checkY < 0 || checkX >= resultMatrix.GetLength(1) || checkY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[pipe.Coord.Y, checkX].Number == -1)
                {
                    rightOutside = true;
                    found = true;
                }

                checkX = pipe.Coord.X + 1;
                checkY = pipe.Coord.Y;
                if (checkX < 0 || checkY < 0 || checkX >= resultMatrix.GetLength(1) || checkY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[pipe.Coord.Y, checkX].Number == -1)
                {
                    rightOutside = false;
                    found = true;
                }
            }

            pipe = pipe.Next;
        }

        pipe = Pipeline;

        while (pipe.Next != null)
        {
            pipe = pipe.Next;
            if (pipe.From == Direction.North)
            {
                var markX = pipe.Coord.X - 1;
                var markY = pipe.Coord.Y;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;

                markX = pipe.Coord.X - 1;
                markY = pipe.Coord.Y - 1;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;
            }

            if (pipe.From == Direction.South)
            {
                var markX = pipe.Coord.X + 1;
                var markY = pipe.Coord.Y;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;

                markX = pipe.Coord.X + 1;
                markY = pipe.Coord.Y + 1;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;
            }

            if (pipe.From == Direction.West)
            {
                var markX = pipe.Coord.X;
                var markY = pipe.Coord.Y + 1;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;

                markX = pipe.Coord.X - 1;
                markY = pipe.Coord.Y + 1;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;
            }

            if (pipe.From == Direction.East)
            {
                var markX = pipe.Coord.X;
                var markY = pipe.Coord.Y - 1;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;

                markX = pipe.Coord.X + 1;
                markY = pipe.Coord.Y - 1;
                if (markX < 0 || markY < 0 || markX >= resultMatrix.GetLength(1) || markY >= resultMatrix.GetLength(0))
                    continue;

                if (resultMatrix[markY, markX].Number == 0)
                    resultMatrix[markY, markX].Number = -1;
            }
        }

    }
    static void MarkInsideClasters()
    {
        ArrayList clusters = new ArrayList();
        List<Point> cluster;
        for (var y = 0; y < resultMatrix.GetLength(0); y++)
        {
            for (var x = 0; x < resultMatrix.GetLength(1); x++)
            {
                if (resultMatrix[y, x].Number == 0 && !resultMatrix[y, x].Visited)
                {
                    cluster = new List<Point>
                    {
                        new Point()
                        {
                            Coord = new Coord() { X = x, Y = y }
                        }
                    };
                    clusters.Add(cluster);
                    ProcessCluster(cluster, -2);
                }
            }
        }
    }

    static void MarkOutSideClasters()
    {
        ArrayList clusters = new ArrayList();
        List<Point> cluster;
        int x = 0, y;
        for (y = 0; y < resultMatrix.GetLength(0); y++)
        {
            if (resultMatrix[y, x].Number == 0 && !resultMatrix[y, x].Visited)
            {
                cluster = new List<Point>
                {
                    new Point()
                    {
                        Coord = new Coord() { X = x, Y = y }
                    }
                };
                clusters.Add(cluster);
                ProcessCluster(cluster);
            }
        }
        x = resultMatrix.GetLength(1) - 1;
        for (y = 0; y < resultMatrix.GetLength(0); y++)
        {
            if (resultMatrix[y, x].Number == 0 && !resultMatrix[y, x].Visited)
            {
                cluster = new List<Point>
                {
                    new Point()
                    {
                        Coord = new Coord() { X = x, Y = y }
                    }
                };
                clusters.Add(cluster);
                ProcessCluster(cluster);
            }
        }

        y = 0;
        for (x = 0; x < resultMatrix.GetLength(1); x++)
        {
            if (resultMatrix[y, x].Number == 0 && !resultMatrix[y, x].Visited)
            {
                cluster = new List<Point>
                {
                    new Point()
                    {
                        Coord = new Coord() { X = x, Y = y }
                    }
                };
                clusters.Add(cluster);
                ProcessCluster(cluster);
            }
        }

        y = resultMatrix.GetLength(0) - 1; ;
        for (x = 0; x < resultMatrix.GetLength(1); x++)
        {
            if (resultMatrix[y, x].Number == 0 && !resultMatrix[y, x].Visited)
            {
                cluster = new List<Point>
                {
                    new Point()
                    {
                        Coord = new Coord() { X = x, Y = y }
                    }
                };
                clusters.Add(cluster);
                ProcessCluster(cluster);
            }
        }
    }

    static void ProcessCluster(List<Point> cluster, int mark = -1)
    {
        var again = false;
        do
        {
            foreach (var point in cluster.Where(x => !x.Visited).ToList())
            {
                point.Visited = true;
                if (!resultMatrix[point.Coord.Y, point.Coord.X].Visited)
                {
                    resultMatrix[point.Coord.Y, point.Coord.X].Visited = true;
                    resultMatrix[point.Coord.Y, point.Coord.X].Number = mark;
                    for (var x = -1; x <= 1; x++)
                        for (var y = -1; y <= 1; y++)
                        {
                            var checkX = point.Coord.X + x;
                            var checkY = point.Coord.Y + y;
                            if (checkX < 0 || checkY < 0 || checkX >= resultMatrix.GetLength(1) || checkY >= resultMatrix.GetLength(0))
                                continue;

                            if (resultMatrix[checkY, checkX].Number == 0 && !resultMatrix[checkY, checkX].Visited &&
                                !cluster.Any(x => x.Coord.X == checkX && x.Coord.Y == checkY))
                            {
                                cluster.Add(new Point()
                                {
                                    Coord = new Coord() { X = checkX, Y = checkY },
                                });
                            }
                        }
                }
            }

            again = cluster.Any(x => !x.Visited);
        }
        while (again);
    }

    public static (Direction from, int x, int y, long number) EnumeratePipeLine(Direction from, int x, int y, long number)
    {
        if (x < 0 || y < 0 || x >= Matrix.GetLength(1) || y >= Matrix.GetLength(0))
            throw new ArgumentException($"out of range: {x},{y}");

        number++;
        if (number > MaxNumber)
        {
            MaxNumber = number;
        }
        switch (Matrix[y, x])
        {
            case '|':
                if (from == Direction.North)
                {
                    resultMatrix[y, x].Number = number;
                    return (Direction.North, x, y + 1, number);
                }
                else if (from == Direction.South)
                {
                    resultMatrix[y, x].Number = number;
                    return (Direction.South, x, y - 1, number);
                }
                break;
            case '-':
                if (from == Direction.West)
                {
                    resultMatrix[y, x].Number = number;
                    return (Direction.West, x + 1, y, number);
                }
                else if (from == Direction.East)
                {
                    resultMatrix[y, x].Number = number;
                    return (Direction.East, x - 1, y, number);
                }
                break;
            case 'L':
                if (from == Direction.North)
                {
                    resultMatrix[y, x].Number = number;
                    return (Direction.West, x + 1, y, number);
                }
                else if (from == Direction.East)
                {
                    resultMatrix[y, x].Number= number;
                    return (Direction.South, x, y - 1, number);
                }
                break;
            case 'J':
                if (from == Direction.North)
                {
                    resultMatrix[y, x].Number= number;
                    return (Direction.East, x - 1, y, number);
                }
                else if (from == Direction.West)
                {
                    resultMatrix[y, x].Number= number;
                    return (Direction.South, x, y - 1, number);
                }
                break;
            case '7':
                if (from == Direction.West)
                {
                    resultMatrix[y, x].Number= number;
                    return (Direction.North, x, y + 1, number);
                }
                else if (from == Direction.South)
                {
                    resultMatrix[y, x].Number= number;
                    return (Direction.East, x - 1, y, number);
                }
                break;
            case 'F':
                if (from == Direction.East)
                {
                    resultMatrix[y, x].Number= number;
                    return (Direction.North, x, y + 1, number);
                }
                else if (from == Direction.South)
                {
                    resultMatrix[y, x].Number= number;
                    return (Direction.West, x + 1, y, number);
                }
                break;
            case '.':
            case 'S':
                break;
            default:
                throw new NotImplementedException();
        }

        throw new ArgumentException($"no match: {x},{y}");
    }
}
