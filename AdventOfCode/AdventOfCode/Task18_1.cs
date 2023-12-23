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
using System.Windows.Input;

namespace AdventOfCode;

public class Task18_1
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task18.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Item
    {
        public char Symbol;
        public int Color;
    }

    class Command
    {
        public int Y;
        public int X;
        public int Steps;
        public int Color;
    }

    Item[,] Matrix;
    List<Command> Commands;

    Dictionary<char, (int y, int x)> CoordDeltaMap = new Dictionary<char, (int y, int x)>
    {
        { 'R', (0, 1) },
        { 'L', (0, -1) },
        { 'D', (1, 0) },
        { 'U', (-1, 0) },
    };

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        Commands = new List<Command>();
        for (var i = 0; i < lines.Length; i++)
        {
            var split = lines[i].Split(' ');
            Commands.Add(new Command
            {
                Y = CoordDeltaMap[split[0][0]].y,
                X = CoordDeltaMap[split[0][0]].x,
                Steps = int.Parse(split[1]),
                Color = int.Parse(split[2][2..^1], System.Globalization.NumberStyles.HexNumber)
            });
            if (i == 100) break;
        }

        Commands.Add(new Command { X = 0, Y = -1, Steps = 20 });
        Commands.Add(new Command { X = 1, Y = 0, Steps = 31 });
        Commands.Add(new Command { X = 0, Y = 1, Steps = 157 });
        Commands.Add(new Command { X = -1, Y = 0, Steps = 1 });

        Matrix = new Item[1500, 1500];

        var currentY = 500;
        var currentX = 100;
        Matrix[currentY, currentX] = new Item { Symbol = '#' };

        var j = 0;
        foreach (var cmd in Commands) 
        {
            for (int i = 1; i <= cmd.Steps; i++)
            {
                currentY += cmd.Y;
                currentX += cmd.X;
                Matrix[currentY, currentX] = new Item { Symbol = '#', Color = cmd.Color };
            }

            j++;
        }

        PrintMatrix("matrix.txt", Matrix);
        Fill(Commands.Count + 1, Commands.Count + 1);
        PrintMatrix("matrix.txt", Matrix);

        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            for (var x = 0; x < Matrix.GetLength(1); x++)
            {
                if (Matrix[y,x] != null)
                    result++;
            }
        }
        return result;
    }

    class Filled
    {
        public int X;
        public int Y;
        public bool Visited;
    }
    void Fill(int y, int x)
    {
        List<Filled> filled = new List<Filled>();
        filled.Add(new Filled { Y = y, X = x });

        do
        {
            foreach (var item in filled.Where(x => !x.Visited).ToList())
            {
                for (var i = -1; i <= 1; i++)
                    for (var j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        var y1 = item.Y + i;
                        var x1 = item.X + j;
                        if (Matrix[y1, x1] == null)
                        {
                            filled.Add(new Filled
                            {
                                X = x1,
                                Y = y1
                            });
                            Matrix[y1, x1] = new Item { Symbol = '#' };
                        }
                    }

                item.Visited = true;
            }


        }
        while (filled.Any(x => !x.Visited));
    }

    static void PrintMatrix(string filename, Item[,] matrix)
    {
        File.Delete(filename);
        for (var y = 0; y < matrix.GetLength(0); y++)
        {
            StringBuilder sb = new StringBuilder();
            for (var x = 0; x < matrix.GetLength(1); x++)
            {
                if (matrix[y, x] != null)
                    sb.Append(matrix[y, x].Symbol);
                else
                    sb.Append(".");
            }
            File.AppendAllText(filename, sb.ToString() + Environment.NewLine);
        }
    }
}
