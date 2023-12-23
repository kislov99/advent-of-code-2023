using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace AdventOfCode;

public class Task18_2
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task18.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Command
    {
        public char Direction;
        public long Steps;
        public int Color;
        public Command Prev;
        public Command Next;
    }

    Command Commands;
    long Result;
    int Total;

    Dictionary<int, char> NumberToDirectionMap = new Dictionary<int, char>
    {
        { 0, 'R' },
        { 1, 'D' },
        { 2, 'L' },
        { 3, 'U' },
    };

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        Commands = new Command();
        var split = lines[0].Split(' ');
        var current = new Command()
        {
            Direction = NumberToDirectionMap[int.Parse(split[2][7..^1])],
            Steps = int.Parse(split[2][2..^2], System.Globalization.NumberStyles.HexNumber),
            Prev = null,
        };

        Commands.Next = current;

        for (var i = 1; i < lines.Length; i++)
        {
            split = lines[i].Split(' ');
            var newCommand = new Command
            {
                Direction = NumberToDirectionMap[int.Parse(split[2][7..^1])],
                Steps = int.Parse(split[2][2..^2], System.Globalization.NumberStyles.HexNumber),
                Prev = current,
            };

            current.Next = newCommand;
            current = newCommand;
            //if (i == 100) break;
        }

        //current.Next = new Command { Direction = 'U', Steps = 20 };
        //current.Next.Prev = current; current = current.Next;
        //current.Next = new Command { Direction = 'R', Steps = 31 };
        //current.Next.Prev = current; current = current.Next;
        //current.Next = new Command { Direction = 'D', Steps = 157 };
        //current.Next.Prev = current; current = current.Next;
        //current.Next = new Command { Direction = 'L', Steps = 7 };
        //current.Next.Prev = current; current = current.Next;

        Total = lines.Length;
        //Total = 104;
        Commands = Commands.Next;
        //Commands = Commands.Next.Next;
        current.Next = Commands;
        Commands.Prev = current;

        current = Commands;

        do
        {
            current = RemoveStraight(current);
            if (Total == 4)
                break;
            var next1 = current.Next;
            var next2 = next1.Next;
            current = ProcessTurn(current, next1, next2);
        }
        while (Total > 4);

        Result += (current.Steps + 1) * (current.Next.Steps + 1);
        return Result;
    }

    bool CheckLoop(Command current, long x1, long x2, long y1, long y2)
    {
        long x = 0;
        long y = 0;
        var newCurrent = current;
        long i = 0;
        do
        {
            RemoveStraight(current);
            switch (newCurrent.Direction)
            {
                case 'U': y -= newCurrent.Steps; break;
                case 'D': y += newCurrent.Steps; break;
                case 'L': x -= newCurrent.Steps; break;
                case 'R': x += newCurrent.Steps; break;
            }
            newCurrent = newCurrent.Next;
            i++;

            if (newCurrent != current && 
                newCurrent != current.Next && 
                newCurrent != current.Next.Next && 
                newCurrent != current.Next.Next.Next &&
                newCurrent != current.Next.Next.Next)
                if (x >= x1 && x < x2 && y >= y1 && y < y2)
                    return true;
        }
        while (newCurrent != current);

        return false;
    }

    (long x1, long x2, long y1, long y2) GetLoopCoords(string seq, Command current)
    {
        switch (seq)
        {
            case "RDL": return (Math.Max(current.Steps - current.Next.Next.Steps, 0), current.Steps, 0, current.Next.Steps); 
            case "LUR": return (-current.Steps, Math.Min(current.Next.Next.Steps - current.Steps, 0), -current.Next.Steps, 0); 
            case "DLU": return (-current.Next.Steps, 0, Math.Max(current.Steps - current.Next.Next.Steps, 0), current.Steps); 
            case "URD": return (0, current.Next.Steps, -current.Steps, Math.Min(current.Next.Next.Steps - current.Steps, 0)); 
        }

        throw new Exception("Inknown direction");
    }

    Command ProcessTurn(Command current, Command c2, Command c3)
    {
        var seq = $"{current.Direction}{c2.Direction}{c3.Direction}";
        if (seq == "RDL" || seq == "DLU" || seq == "LUR" || seq == "URD")
        //if (seq == "LDR" || seq == "ULD" || seq == "RUL" || seq == "DRU")
        {
            (long x1, long x2, long y1, long y2) = GetLoopCoords(seq, current);
            if (x1 > x2 || y1 > y2)
                throw new Exception("x1 > x2 || y1 > y2");

            if (!CheckLoop(current, x1, x2, y1, y2))
            {
                if (current.Steps > c3.Steps)
                {
                    Total--;
                    Result += c3.Steps * (c2.Steps + 1);
                    current.Steps -= c3.Steps;
                    c2.Next = c3.Next;
                    c3.Next.Prev = c2;
                    return current;
                }
                else if (current.Steps < c3.Steps)
                {
                    Total--;
                    Result += current.Steps * (c2.Steps + 1);
                    c3.Steps -= current.Steps;
                    current.Prev.Next = c2;
                    c2.Prev = current.Prev;
                    return current.Prev;
                }
                else
                {
                    Total-=3;

                    Result += c3.Steps * (c2.Steps + 1);
                    current.Prev.Next = c3.Next;
                    current.Prev.Steps += c2.Steps;
                    c3.Next.Prev = current.Prev;
                    return current.Prev;
                }
            }
        }

        return current.Next;
    }

    Command RemoveStraight(Command current)
    {
        while ((current.Direction == current.Next.Direction) ||
            ((current.Direction == 'D' && current.Next.Direction == 'U') ||
                (current.Direction == 'U' && current.Next.Direction == 'D') ||
                (current.Direction == 'R' && current.Next.Direction == 'L') ||
                (current.Direction == 'L' && current.Next.Direction == 'R')))
        {
            if (current.Direction == current.Next.Direction)
            {
                current.Steps += current.Next.Steps;
                current.Next = current.Next.Next;
                current.Next.Prev = current;
                Total--;
            }

            if ((current.Direction == 'D' && current.Next.Direction == 'U') ||
                (current.Direction == 'U' && current.Next.Direction == 'D') ||
                (current.Direction == 'R' && current.Next.Direction == 'L') ||
                (current.Direction == 'L' && current.Next.Direction == 'R'))
            {
                if (current.Steps > current.Next.Steps)
                {
                    Result += current.Next.Steps;
                    current.Steps -= current.Next.Steps;
                    current.Next = current.Next.Next;
                    current.Next.Prev = current;
                    Total--;
                }
                else if (current.Steps < current.Next.Steps)
                {
                    Result += current.Steps;
                    current.Steps = current.Next.Steps - current.Steps;
                    current.Direction = current.Next.Direction;
                    current.Next = current.Next.Next;
                    current.Next.Prev = current;
                    Total--;
                }
                else
                {
                    current.Prev.Next = current.Next.Next;
                    current.Next.Next.Prev = current.Prev;
                    Total -=2;
                }
                current = current.Prev;
            }
            //PrintMatrix("matrix.txt", current);
        }

        return current;
    }

    char[,] Matrix;

    Dictionary<char, (int y, int x)> CoordDeltaMap = new Dictionary<char, (int y, int x)>
    {
        { 'R', (0, 1) },
        { 'L', (0, -1) },
        { 'D', (1, 0) },
        { 'U', (-1, 0) },
    };

    void PrintMatrix(string filename, Command current)
    {
        Matrix = new char[1500, 1500];
        var currentY = 500;
        var currentX = 100;
        Matrix[currentY, currentX] = '#';
        var j = 0;
        var newCurrent = current;
        do
        {
            for (int i = 1; i <= newCurrent.Steps; i++)
            {
                var Y = CoordDeltaMap[newCurrent.Direction].y;
                var X = CoordDeltaMap[newCurrent.Direction].x;
                currentY += Y;
                currentX += X;
                Matrix[currentY, currentX] = '#';
            }
            newCurrent = newCurrent.Next;
        }
        while (newCurrent != current);

        File.Delete(filename);
        for (var y = 0; y < Matrix.GetLength(0); y++)
        {
            StringBuilder sb = new StringBuilder();
            for (var x = 0; x < Matrix.GetLength(1); x++)
            {
                if (Matrix[y, x] != 0)
                    sb.Append(Matrix[y, x]);
                else
                    sb.Append(".");
            }
            File.AppendAllText(filename, sb.ToString() + Environment.NewLine);
        }
    }
}
