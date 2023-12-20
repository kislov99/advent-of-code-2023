using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task16_2
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task16.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Item
    {
        public char Symbol;
        public char Energized = '.';
        public List<Direction> Directions = new List<Direction>();
    }

    class Beam 
    { 
        public Coord Coord;
        public Direction Direction;
        public Beam Prev;
        public bool Finished;
    }

    static Item[,] Matrix;
    static List<Beam> Beams = new List<Beam>();

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        Matrix = new Item[lines[0].Length, lines.Length];
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                Matrix[y, x] = new Item { Symbol = lines[y][x] };
            }
        }

        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            var res = Run(y, -1, Direction.East);
            if (res > result)
                result = res;
        }
        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            var res = Run(y, Matrix.GetLength(1), Direction.West);
            if (res > result)
                result = res;
        }
        for (var x = 0; x < Matrix.GetLength(1); x++)
        {
            var res = Run(-1, x, Direction.South);
            if (res > result)
                result = res;
        }
        for (var x = 0; x < Matrix.GetLength(1); x++)
        {
            var res = Run(Matrix.GetLength(0), x, Direction.North);
            if (res > result)
                result = res;
        }
        PrintMatrix("matrix.txt", Matrix);

        return result;
    }

    int Run(int y, int x, Direction direction)
    {
        Beams = new List<Beam>
        {
            new Beam
            {
                Coord = new Coord { Y = y, X = x },
                Direction = direction
            }
        };

        while (Beams.Any(x => !x.Finished))
        {
            for (var i = 0; i < Beams.Count; i++)
            {
                if (Beams[i].Finished)
                    continue;

                switch (Beams[i].Direction)
                {
                    case Direction.East: Beams[i] = East(Beams[i]); break;
                    case Direction.South: Beams[i] = South(Beams[i]); break;
                    case Direction.North: Beams[i] = North(Beams[i]); break;
                    case Direction.West: Beams[i] = West(Beams[i]); break;
                    default: throw new ArgumentException();
                };
            }
        }

        var result = 0;
        for (y = 0; y < Matrix.GetLength(0); y++)
        {
            for (x = 0; x < Matrix.GetLength(1); x++)
            {
                result += Matrix[y, x].Energized == '.' ? 0 : 1;
                Matrix[y, x].Energized = '.';
                Matrix[y, x].Directions = new List<Direction>();
            }
        }

        return result;
    }

    Beam East(Beam beam)
    {
        var y = beam.Coord.Y;
        var x = beam.Coord.X + 1;
        if (x >= Matrix.GetLength(1) ||
            Matrix[y, x].Directions.Contains(Direction.East))
        {
            beam.Finished = true;
            return beam;
        }

        Matrix[y, x].Energized = '>';
        Matrix[y, x].Directions.Add(Direction.East);

        var coord = new Coord { Y = y, X = x };

        var newBeam = new Beam
        {
            Coord = coord,
            Direction = beam.Direction,
            Prev = beam
        };

        if (Matrix[y, x].Symbol == '\\')
        {
            newBeam.Direction = Direction.South;
            Matrix[y, x].Energized = '\\';
        }
        else if (Matrix[y, x].Symbol == '/')
        {
            newBeam.Direction = Direction.North;
            Matrix[y, x].Energized = '/';
        }
        else if (Matrix[y, x].Symbol == '|')
        {
            Matrix[y, x].Energized = '|';
            newBeam.Direction = Direction.North;

            var newBeam2 = new Beam
            {
                Coord = coord,
                Direction = Direction.South,
                Prev = beam
            };
            Beams.Add(newBeam2);
        }
        return newBeam;
    }

    Beam West(Beam beam)
    {
        var y = beam.Coord.Y;
        var x = beam.Coord.X - 1;
        if (x < 0 ||
            Matrix[y, x].Directions.Contains(Direction.West))
        {
            beam.Finished = true;
            return beam;
        }

        Matrix[y, x].Energized = '<';
        Matrix[y, x].Directions.Add(Direction.West);

        var coord = new Coord { Y = y, X = x };

        var newBeam = new Beam
        {
            Coord = coord,
            Direction = beam.Direction,
            Prev = beam
        };
        
        if (Matrix[y, x].Symbol == '\\')
        {
            newBeam.Direction = Direction.North;
            Matrix[y, x].Energized = '\\';
        }
        else if (Matrix[y, x].Symbol == '/')
        {
            newBeam.Direction = Direction.South;
            Matrix[y, x].Energized = '/';
        }
        else if (Matrix[y, x].Symbol == '|')
        {
            Matrix[y, x].Energized = '|';
            newBeam.Direction = Direction.North;

            var newBeam2 = new Beam
            {
                Coord = coord,
                Direction = Direction.South,
                Prev = beam
            };

            Beams.Add(newBeam2);
        }
        return newBeam;
    }

    Beam North(Beam beam)
    {
        var y = beam.Coord.Y - 1;
        var x = beam.Coord.X;

        if (y < 0 ||
            Matrix[y, x].Directions.Contains(Direction.North))
        {
            beam.Finished = true;
            return beam;
        }

        Matrix[y, x].Energized = '^';
        Matrix[y, x].Directions.Add(Direction.North);

        var coord = new Coord { Y = y, X = x };

        var newBeam = new Beam
        {
            Coord = coord,
            Direction = beam.Direction,
            Prev = beam
        };

        if (Matrix[y, x].Symbol == '\\')
        {
            newBeam.Direction = Direction.West;
            Matrix[y, x].Energized = '\\';
        }
        else if (Matrix[y, x].Symbol == '/')
        {
            newBeam.Direction = Direction.East;
            Matrix[y, x].Energized = '/';
        }
        else if (Matrix[y, x].Symbol == '-')
        {
            Matrix[y, x].Energized = '-';
            newBeam.Direction = Direction.West;

            var newBeam2 = new Beam
            {
                Coord = coord,
                Direction = Direction.East,
                Prev = beam
            };

            Beams.Add(newBeam2);
        }
        return newBeam;
    }

    Beam South(Beam beam)
    {
        var y = beam.Coord.Y + 1;
        var x = beam.Coord.X;

        if (y >= Matrix.GetLength(0) ||
            Matrix[y, x].Directions.Contains(Direction.South))
        {
            beam.Finished = true;
            return beam;
        }

        Matrix[y, x].Energized = 'v';
        Matrix[y, x].Directions.Add(Direction.South);

        var coord = new Coord { Y = y, X = x };

        var newBeam = new Beam
        {
            Coord = coord,
            Direction = beam.Direction,
            Prev = beam
        };

        if (Matrix[y, x].Symbol == '\\')
        {
            newBeam.Direction = Direction.East;
            Matrix[y, x].Energized = '\\';
        }
        else if (Matrix[y, x].Symbol == '/')
        {
            newBeam.Direction = Direction.West;
            Matrix[y, x].Energized = '/';
        }
        else if (Matrix[y, x].Symbol == '-')
        {
            newBeam.Direction = Direction.East;
            Matrix[y, x].Energized = '-';

            var newBeam2 = new Beam
            {
                Coord = coord,
                Direction = Direction.West,
                Prev = beam
            };

            Beams.Add(newBeam2);
        }
        return newBeam;
    }

    static void PrintMatrix(string filename, Item[,] matrix)
    {
        File.Delete(filename);
        for (var y = 0; y < matrix.GetLength(0); y++)
        {
            for (var x = 0; x < matrix.GetLength(1); x++)
            {
                File.AppendAllText(filename, matrix[y, x].Energized +  " ");
            }
            File.AppendAllText(filename, Environment.NewLine);
        }
    }

}
