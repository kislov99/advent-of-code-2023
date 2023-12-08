using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task6_2
{ 
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task6.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    public static long ProcessLines(string[] lines)
    {
        var time = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
        var distance = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));

        var wins = 0;
        for (var i = 1; i < time; i++)
        {
            if (i * (time - i) > distance)
            {
                wins++;
            }
        }

        return wins;
    }
}