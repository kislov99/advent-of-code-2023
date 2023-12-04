using Microsoft.VisualBasic;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task4_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task4.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static Dictionary<int, int> wins = new Dictionary<int, int>();

    public static int ProcessLines(string[] lines)
    {
        int result;
        for (int i=0; i< lines.Length; i++)
        {
            result = 0;
            Regex regex = new Regex("Card.+: (.+)");
            var gameMatch = regex.Match(lines[i]);

            if (gameMatch.Success)
            {
                var numberLines = gameMatch.Groups[1].Value;
                var groups = numberLines.Split("|");
                var winningNumbers = groups[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim()));
                var myNumbers = groups[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim()));

                foreach (var myNumber in myNumbers)
                {
                    if (winningNumbers.Contains(myNumber))
                    {
                        result++;
                    }
                }

            }
            wins[i] = result;
        }

        result = 0;
        for (int i = 0; i < wins.Count; i++)
        {
            result++;
            result = ProcessWin(result, i);
        }

        return result;
    }

    static int ProcessWin(int result, int i) 
    {
        if (wins[i] > 0)
        {
            result += wins[i];
            for (int j = i + 1; j <= Math.Min(i + wins[i], wins.Count - 1); j++)
            {
                result = ProcessWin(result, j);
            }
        }
        return result;
    }
}