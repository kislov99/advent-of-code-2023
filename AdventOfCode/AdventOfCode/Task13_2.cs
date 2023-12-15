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

public class Task13_2
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task13.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static List<M> Matricies = new List<M>();
    class M
    {
        public List<string> Matrix = new List<string>();
        public int Symmetry;
        public bool Horizonatal;
    }

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        {
            var matrix = new M();
            for (var y = 0; y < lines.Length; y++)
            {
                if (lines[y] != string.Empty)
                    matrix.Matrix.Add(lines[y]);
                else
                {
                    Matricies.Add(matrix);
                    matrix = new M();
                }
            }
            Matricies.Add(matrix);
        }

        foreach (var m in Matricies)
        {
            for (var y = 0; y < m.Matrix.Count - 1; y++)
            {
                var symmetry = true;
                var diff = new List<Coord>();
                for (var y1 = 0; y - y1 >= 0 && y + y1 + 1 < m.Matrix.Count; y1++)
                {
                    for (var i = 0; i < m.Matrix[y - y1].Length; i++)
                    {
                        if (m.Matrix[y - y1][i] != m.Matrix[y + y1 + 1][i])
                        {
                            if (m.Matrix[y - y1][i] == '.')
                                diff.Add(new Coord { Y = y - y1, X = i });
                            else
                                diff.Add(new Coord { Y = y + y1 + 1, X = i }); 
                        }
                    }
                    if (diff.Count > 1)
                    {
                        symmetry = false;
                        break;
                    }
                }

                if (diff.Count == 1)
                {
                    m.Symmetry = y + 1;
                    m.Horizonatal = true;
                    break;
                }
            }
        }

        foreach (var m in Matricies)
        {
            if (m.Symmetry == 0)
            {
                for (var x = 0; x < m.Matrix[0].Length - 1; x++)
                {
                    var symmetry = true;
                    var diff = new List<Coord>();
                    for (var x1 = 0; x - x1 >= 0 && x + x1 + 1 < m.Matrix[0].Length; x1++)
                    {
                        for (var i = 0; i < m.Matrix.Count; i++)
                        {
                            if (m.Matrix[i][x - x1] != m.Matrix[i][x + x1 + 1])
                            {
                                if (m.Matrix[i][x - x1] == '.')
                                    diff.Add(new Coord { Y = i, X = x - x1 });
                                else
                                    diff.Add(new Coord { Y = i, X = x + x1 + 1 });
                            }
                        }
                        if (diff.Count > 1)
                        {
                            symmetry = false;
                            break;
                        }
                    }

                    if (diff.Count == 1)
                    {
                        m.Symmetry = x + 1;
                        m.Horizonatal = false;
                        break;
                    }
                }
            }
        }

        foreach (var m in Matricies)
        {
            if (m.Horizonatal)
                result += 100* m.Symmetry;
            else
                result += m.Symmetry;
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
