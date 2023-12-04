using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task2_1
{
    static string[] digits = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task2.txt");
        var sum = 0;

        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }

        Console.WriteLine(sum);
    }

    public static int ProcessLine(string input)
    {
        Regex regex = new Regex("Game (\\d+): (.+)");
        var gameMatch = regex.Match(input);
        string groupId = "0";

        if (gameMatch.Success)
        {
            groupId = gameMatch.Groups[1].Value;
            var cubesLine = gameMatch.Groups[2].Value;
            var groups = cubesLine.Split(";");
            foreach (var group in groups)
            {
                var skip = false;
                var sets = group.Split(",");
                foreach (var set in sets)
                {
                    Regex greenRegex = new Regex("(\\d+) green");
                    Regex blueRegex = new Regex("(\\d+) blue");
                    Regex redRegex = new Regex("(\\d+) red");

                    var redMatch = redRegex.Match(set.Trim());
                    if (redMatch.Success)
                    {
                        if (int.TryParse(redMatch.Groups[1].Value, out var redNumber))
                        {
                            if (redNumber > 12)
                            {
                                skip = true;
                                break;
                            }
                        }
                    }

                    var greenMatch = greenRegex.Match(set.Trim());
                    if (greenMatch.Success)
                    {
                        if (int.TryParse(greenMatch.Groups[1].Value, out var greenNumber))
                        {
                            if (greenNumber > 13)
                            {
                                skip = true;
                                break;
                            }

                        }
                    }

                    var blueMatch = blueRegex.Match(set.Trim());
                    if (blueMatch.Success)
                    {
                        if (int.TryParse(blueMatch.Groups[1].Value, out var blueNumber))
                        {
                            if (blueNumber > 14)
                            {
                                skip = true;
                                break;
                            }
                        }
                    }
                }
                if (skip)
                {
                    groupId = "0";
                    break;
                }
            }
        }

        return int.Parse(groupId);
    }
}