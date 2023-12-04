using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode;

public class Task3_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task3.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    public static int ProcessLines(string[] lines)
    {

        var result = 0;
        for(int x1=0; x1 < lines.Length; x1++)
        {
            var number = 0;
            var position1 = -1;
            var position2 = -1;

            var currentline = lines[x1];
            for(int y1 = 0; y1 < currentline.Length; y1++)
            {
                if (int.TryParse(currentline[y1].ToString(), out var digit))
                {
                    if (number != 0)
                    {
                        position2 = y1;
                        number = 10 * number;
                    }
                    else
                    {
                        position2 = position1 = y1;
                    }
                    number += digit;

                    if (y1 != currentline.Length - 1)
                        continue;
                }

                if (number != 0)
                {
                    for (int x2 = x1 - 1; x2 <= x1 + 1; x2++)
                    {
                        if (x2 >= 0 & x2 < lines.Length)
                        {
                            var adjucentLine = lines[x2];
                            for (int y2 = position1 - 1; y2 <= position2 + 1; y2++)
                            {
                                if (y2 >= 0 & y2 < adjucentLine.Length)
                                {
                                    if (!int.TryParse(adjucentLine[y2].ToString(), out _) && adjucentLine[y2] != '.')
                                    {
                                        AddGear(x2, y2, number);                                        
                                    }
                                }
                            }
                        }
                    }

                    number = 0;
                    position1 = -1;
                    position2 = -1;
                }
            }
        }

        foreach (var numbers in gears.Values)
        {
            if (numbers.Count == 2)
            {
                result += numbers[0] * numbers[1];
            }
        }
        return result;
    }

    static Dictionary<string, List<int>> gears = new Dictionary<string, List<int>>();
    static void AddGear(int pos1, int pos2, int number)
    {
        var index = $"{pos1}_{pos2}";
        if (!gears.ContainsKey(index))
        {
            gears[index] = new List<int>() { number };
        }
        else
        {
            gears[index].Add(number);
        }
    }
}