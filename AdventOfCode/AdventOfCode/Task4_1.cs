using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task4_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task4.txt");
        var sum = 0;

        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }

        Console.WriteLine(sum);
    }

    public static int ProcessLine(string input)
    {
        var result = 0;
        Regex regex = new Regex("Card.+: (.+)");
        var gameMatch = regex.Match(input);

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
                    if (result == 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result *= 2;
                    }
                }
            }

        }

        return result;
    }
}