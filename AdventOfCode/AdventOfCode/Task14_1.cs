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

public class Task14_1
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task14.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static char[,] OldMatrix;
    static char[,] Matrix;

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        Coord start = new Coord();
        Matrix = new char[lines[0].Length, lines.Length];
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                Matrix[y, x] = lines[y][x];
            }
        }

        var cycles = 1000000000;
        string hash = "", oldhash = "";
        Save();
        bool calcHash = true;
        for (long i = 0; i <= cycles; i++)
        {
            if (calcHash)
                hash = Hash(Direction.North);

            if (!cache2.ContainsKey(oldhash))
            {
                //result = 0;
                //for (var y = 0; y < Matrix.GetLength(0); y++)
                //{
                //    for (var x = 0; x < Matrix.GetLength(1); x++)
                //    {
                //        if (Matrix[y, x] == 'O')
                //        {
                //            result += Matrix.GetLength(0) - y;
                //        }
                //    }
                //}
                cache2.Add(oldhash, (hash,OldMatrix, result));
                oldhash = hash;
            }
            else
            {
                (hash, Matrix, result) = cache2[oldhash];
                oldhash = hash;
                calcHash = false;

                continue;
            }
            North();                 
            West();
            South();
            East();
            Save();

            //if (cache.TryGetValue(hash, out var tmp))
            //{
            //    Matrix = tmp;
            //    Save();
            //}
            //else
            //{
            //    North();
            //    Save();
            //    cache.Add(hash, OldMatrix);
            //}

            //hash = Hash(Direction.West);
            //if (cache.TryGetValue(hash, out tmp))
            //{
            //    Matrix = tmp;
            //    Save();
            //}
            //else
            //{
            //    West();
            //    Save();
            //    cache.Add(hash, OldMatrix);
            //}

            //hash = Hash(Direction.South);
            //if (cache.TryGetValue(hash, out tmp))
            //{
            //    Matrix = tmp;
            //    Save();
            //}
            //else
            //{
            //    South();
            //    Save();
            //    cache.Add(hash, OldMatrix);
            //}

            //hash = Hash(Direction.East);
            //if (cache.TryGetValue(hash, out tmp))
            //{
            //    Matrix = tmp;
            //    Save();
            //}
            //else
            //{
            //    East();
            //    Save();
            //    cache.Add(hash, OldMatrix);
            //}

            //PrintMatrix($"matrix{i}.txt", Matrix);

            //result = 0;
            //for (var y = 0; y < Matrix.GetLength(0); y++)
            //{
            //    for (var x = 0; x < Matrix.GetLength(1); x++)
            //    {
            //        if (Matrix[y, x] == 'O')
            //        {
            //            result += Matrix.GetLength(0) - y;
            //        }
            //    }
            //}
        }

        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            for (var x = 0; x < Matrix.GetLength(1); x++)
            {
                if (Matrix[y, x] == 'O')
                {
                    result += Matrix.GetLength(0) - y; 
                }
            }
        }

        PrintMatrix("matrix.txt", Matrix);

        return result;
    }

    string Hash(Direction direction)
    {
        var result = new StringBuilder();
        for (var y = 0; y < OldMatrix.GetLength(0); y++)
        {
            for (var x = 0; x < OldMatrix.GetLength(1); x++)
            {
                result.Append(OldMatrix[y, x]);
            }
        }
        result.Append(direction);
        using (HashAlgorithm algorithm = SHA256.Create())
        return Encoding.UTF8.GetString(algorithm.ComputeHash(Encoding.UTF8.GetBytes(result.ToString())));
    }

    void Save()
    {
        OldMatrix = new char[Matrix.GetLength(0), Matrix.GetLength(1)];
        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            for (var x = 0; x < Matrix.GetLength(1); x++)
            {
                OldMatrix[y,x] = Matrix[y,x];
            }
        }
    }
    Dictionary<string, char[,]> cache = new Dictionary<string, char[,]>();
    Dictionary<string,(string, char[,], int)> cache2 = new Dictionary<string, (string, char[,], int)>();

    void North()
    {
        for (var x = 0; x < Matrix.GetLength(1); x++)
        {
            for (var y = 1; y < Matrix.GetLength(0); y++)
            {
                var y1 = y;
                if (Matrix[y1, x] == 'O')
                {
                    while (y1 >= 1 && Matrix[y1 - 1, x] == '.')
                    {
                        Matrix[y1 - 1, x] = 'O';
                        Matrix[y1, x] = '.';
                        y1--;
                    }
                }
            }
        }
    }

    void South()
    {
        for (var x = 0; x < Matrix.GetLength(1); x++)
        {
            for (var y = Matrix.GetLength(0) - 2; y >=0; y--)
            {
                var y1 = y;
                if (Matrix[y1, x] == 'O')
                {
                    while (y1 <= Matrix.GetLength(0) - 2 && Matrix[y1 + 1, x] == '.')
                    {
                        Matrix[y1 + 1, x] = 'O';
                        Matrix[y1, x] = '.';
                        y1++;
                    }
                }
            }
        }
    }

    void West()
    {
        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            for (var x = 1; x < Matrix.GetLength(1); x++)
            {
                var x1 = x;
                if (Matrix[y, x1] == 'O')
                {
                    while (x1 >= 1 && Matrix[y, x1 - 1] == '.')
                    {
                        Matrix[y, x1 - 1] = 'O';
                        Matrix[y, x1] = '.';
                        x1--;
                    }
                }
            }
        }
    }

    void East()
    {
        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            for (var x = Matrix.GetLength(1) - 2; x >=0; x--)
            {
                var x1 = x;
                if (Matrix[y, x1] == 'O')
                {
                    while (x1 <= Matrix.GetLength(1) - 2 && Matrix[y, x1 + 1] == '.')
                    {
                        Matrix[y, x1 + 1] = 'O';
                        Matrix[y, x1] = '.';
                        x1++;
                    }
                }
            }
        }
    }

    static void PrintMatrix(string filename, char[,] matrix)
    {
        File.Delete(filename);
        for (var y = 0; y < matrix.GetLength(0); y++)
        {
            for (var x = 0; x < matrix.GetLength(1); x++)
            {
                File.AppendAllText(filename, matrix[y, x] +  " ");
            }
            File.AppendAllText(filename, Environment.NewLine);
        }
    }

}
