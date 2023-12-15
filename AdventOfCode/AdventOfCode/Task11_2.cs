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

public class Task11_2
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task11.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static char[,] Matrix;
    List<int> EmptyRows = new List<int>();
    List<int> EmptyCols = new List<int>();

    static List<Coord> galaxies = new List<Coord>();

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        Coord start = new Coord();
        Matrix = new char[lines[0].Length,lines.Length];
        for (var y = 0; y<lines.Length; y++)
        {
            for (var x=0; x < lines[y].Length;x++)
            {
                Matrix[y,x] = lines[y][x];
            }
        }

        Expand();

        FindGalaxies();

        return CalculateDistances();
    }

    void Expand()
    {
        for (var x = 0; x < Matrix.GetLength(1); x++)
        {
            bool allEmpty = true;
            for (var y = 0; y < Matrix.GetLength(0); y++)
            {
                if (Matrix[y, x] != '.')
                    allEmpty = false;
            }

            if (allEmpty)
            {
                EmptyCols.Add(x);
            }
        }

        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            bool allEmpty = true;
            for (var x = 0; x < Matrix.GetLength(1); x++)
            {
                if (Matrix[y, x] != '.')
                    allEmpty = false;
            }

            if (allEmpty)
            {
                EmptyRows.Add(y);
            }
        }
    }

    void FindGalaxies()
    {
        const int Expansion = 1000000;
        var newY = 0;
        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            if (EmptyRows.Contains(y))
                newY += Expansion;
            else
                newY++;
            var newX = 0;
            for (var x = 0; x < Matrix.GetLength(1); x++)
            {
                if (EmptyCols.Contains(x))
                    newX += Expansion;
                else
                    newX++;

                if (Matrix[y, x] == '#')
                {
                    galaxies.Add(new Coord() { X = newX, Y = newY });
                }
            }
        } 
    }

    long CalculateDistances()
    {
        long result = 0;
        for (var y = 0; y < galaxies.Count; y++)
        {
            for (var x = y + 1; x < galaxies.Count; x++)
            {
                result +=
                    Math.Abs(galaxies[x].Y - galaxies[y].Y) +
                    Math.Abs(galaxies[x].X - galaxies[y].X);
            }
        }

        return result;
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
