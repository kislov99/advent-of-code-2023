using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task8_1
{
    public class StepData
    {
        public string Left;
        public string Right;
    }

    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task8.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    public static long ProcessLines(string[] lines)
    {
        var directions = lines[0];
        var map = new Dictionary<string, StepData>();
        var regex = new Regex("(...) = .(...), (...).");


        for (var i = 2; i < lines.Length; i++)
        {
            var match = regex.Match(lines[i]);
            if (match.Success)
            {
                map.Add(match.Groups[1].Value, new StepData() 
                {
                    Left = match.Groups[2].Value,
                    Right = match.Groups[3].Value
                });                
            }
        }

        var steps = 0;
        var current = "AAA";
        while (current != "ZZZ")
        {
            foreach (var direction in directions)
            {
                current = direction switch
                {
                    'L' => map[current].Left,
                    'R' => map[current].Right
                };
                steps++;
            }
        }
        return steps;
    }
}
