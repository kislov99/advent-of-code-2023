using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public struct Coord
{
    public int X;
    public int Y;
}

public enum Direction
{
    North,
    East,
    South,
    West,
}

public class Task10_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task10.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static char[,] Matrix;
    static long[,] resultMatrix;

    public static long ProcessLines(string[] lines)
    {
        var result = 0;
        Coord start = new Coord();
        Matrix = new char[lines[0].Length,lines.Length];
        resultMatrix = new long[lines[0].Length, lines.Length];
        for (var y = 0; y<lines.Length; y++)
        {
            for (var x=0; x < lines[y].Length;x++)
            {
                var c= lines[y][x];
                if (c == 'S')
                {
                    start.X = x;
                    start.Y = y;
                }
                Matrix[y,x] = c;
            }
        }
        {
            var x = start.X + 1;
            var y = start.Y;
            Direction from = Direction.West;
            long number = 0;
            while (Matrix[y, x] != 'S')
            {
                (from, x, y, number) = Go(from, x, y, number);
            }
        }
        return (MaxNumber + 1)/2;
    }

    static long MaxNumber;
    public static (Direction from, int x, int y, long number) Go(Direction from, int x, int y, long number)
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
                    resultMatrix[y, x] = number;
                    return (Direction.North, x, y + 1, number);
                }
                else if (from == Direction.South)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.South, x, y - 1, number);
                }
                break;
            case '-':
                if (from == Direction.West)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.West, x + 1, y, number);
                }
                else if (from == Direction.East)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.East, x - 1, y, number);
                }
                break;
            case 'L':
                if (from == Direction.North)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.West, x + 1, y, number);
                }
                else if (from == Direction.East)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.South, x, y - 1, number);
                }
                break;
            case 'J':
                if (from == Direction.North)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.East, x - 1, y, number);
                }
                else if (from == Direction.West)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.South, x, y - 1, number);
                }
                break;
            case '7':
                if (from == Direction.West)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.North, x, y + 1, number);
                }
                else if (from == Direction.South)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.East, x - 1, y, number);
                }
                break;
            case 'F':
                if (from == Direction.East)
                {
                    resultMatrix[y, x] = number;
                    return (Direction.North, x, y + 1, number);
                }
                else if (from == Direction.South)
                {
                    resultMatrix[y, x] = number;
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
