using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task6_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task6.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    public static long ProcessLines(string[] lines)
    {
        var times = lines[0].Split(':')[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();
        var distances = lines[1].Split(':')[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();
        var result = 1;

        for (var t = 0; t < times.Count; t++)
        {
            var wins = 0;
            for (var i = 1; i < times[t]; i++)
            {
                if (i * (times[t] - i) > distances[t])
                {
                    wins++;
                }
            }

            result *= wins;
        }
        return result;
    }
}