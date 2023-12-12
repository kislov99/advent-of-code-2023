using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task9_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task9.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    public static long ProcessLines(string[] lines)
    {
        var result = 0;
        foreach (var line in lines)
        {
            ArrayList list = new ArrayList();
            var array = line.Split(" ").Select(x => int.Parse(x)).ToArray();
            int[] newArray;
            do
            {
                list.Add(array);
                newArray = new int[array.Count() - 1];
                for (int i = 0; i < array.Count() - 1; i++)
                {
                    newArray[i] = array[i+1] - array[i];
                }
                array = newArray;
            }
            while (newArray.Any(x => x != 0));

            var matrix = list.ToArray();
            var firstValue = 0;
            var lineResult = 0;
            for (var i = matrix.Length - 1; i >= 0; i--)
            {
                var finalArray = (int[])matrix[i];
                firstValue = finalArray[0];
                lineResult = firstValue - lineResult;
            }

            result += lineResult;
        }

        return result;
    }
}
