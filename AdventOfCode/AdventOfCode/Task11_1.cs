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

public class Task11_1
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task11.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static char[,] Matrix;
    static char[,] ExpandedMatrix1;
    static char[,] ExpandedMatrix2;
    int SizeX, SizeY;
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
        ExpandedMatrix1 = new char[Matrix.GetLength(0) * 2, Matrix.GetLength(1) * 2];
        ExpandedMatrix2 = new char[Matrix.GetLength(0) * 2, Matrix.GetLength(1) * 2];
        for (var x = 0; x < Matrix.GetLength(1); x++)
        {
            bool allEmpty = true;
            for (var y = 0; y < Matrix.GetLength(0); y++)
            {
                if (Matrix[y, x] != '.')
                    allEmpty = false;
                ExpandedMatrix1[y, SizeX] = Matrix[y, x];
            }
            SizeX++;

            if (allEmpty)
            {
                for (var y = 0; y < Matrix.GetLength(0); y++)
                {
                    ExpandedMatrix1[y, SizeX] = Matrix[y, x];
                }
                SizeX++;
            }
        }

        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            bool allEmpty = true;
            for (var x = 0; x < SizeX; x++)
            {
                if (ExpandedMatrix1[y, x] != '.')
                    allEmpty = false;
                ExpandedMatrix2[SizeY, x] = ExpandedMatrix1[y, x];
            }
            SizeY++;

            if (allEmpty)
            {
                for (var x = 0; x < ExpandedMatrix1.GetLength(1); x++)
                {
                    ExpandedMatrix2[SizeY, x] = ExpandedMatrix1[y, x];
                }
                SizeY++;
            }
        }
    }

    void FindGalaxies()
    {
        for (var y = 0; y < SizeY; y++)
        {
            for (var x = 0; x < SizeX; x++)
            {
                if (ExpandedMatrix2[y, x] == '#')
                {
                    galaxies.Add(new Coord() { X = x, Y = y });
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
                result += Math.Abs(galaxies[x].Y - galaxies[y].Y) + Math.Abs(galaxies[x].X - galaxies[y].X);
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
