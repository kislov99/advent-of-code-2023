using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task5_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task5.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    static Dictionary<int, List<GardenMap>> Maps = new Dictionary<int, List<GardenMap>>();

    public static long ProcessLines(string[] lines)
    {
        long result = long.MaxValue;
        var seeds = Array.Empty<long>();
        int mapId = 0;

        Regex regex = new Regex("seeds: (.+)");
        var seedsMatch = regex.Match(lines[0]);

        if (seedsMatch.Success)
        {
            seeds = seedsMatch.Groups[1].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim())).ToArray<long>();
        }

        int i = 1;
        do
        {
            i+=2;
            Maps[mapId] = new List<GardenMap>();
            do
            {
                var lineValues = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim())).ToList();
                Maps[mapId].Add(
                    new GardenMap
                    {
                        Source = lineValues[1],
                        Destination = lineValues[0],
                        Range = lineValues[2]
                    }
                );
                i++;

                if (i == lines.Length)
                    break;
            }
            while (lines[i] != "");
            mapId++;
        }
        while (i != lines.Length);

        long seed;
        for (var n=0; n < seeds.Length; n+=2)
        {
            for (seed = seeds[n]; seed < seeds[n] + seeds[n + 1]; seed++)
            {
                var mappedValue = seed;
                foreach (var map in Maps.Values)
                {
                    foreach (var item in map)
                    {
                        if (item.Source <= mappedValue && mappedValue <= item.Source + item.Range)
                        {
                            mappedValue = mappedValue - item.Source + item.Destination;
                            break;
                        }
                    }
                }
                result = Math.Min(mappedValue, result);
            }
        }

        return result;
    }
}