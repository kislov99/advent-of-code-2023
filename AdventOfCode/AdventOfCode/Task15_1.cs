using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task15_1
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task15.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Step
    {
        public string Label;
        public int Focal;
    }

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        var sequences = lines[0].Split(',');

        var Boxes = new List<Step>[256];
        for(var i=0; i < 256; i++)
            Boxes[i] = new List<Step>();

        foreach (var seq in sequences)
        {
            if (seq.IndexOf('-') > 0)
            {
                var label = seq[..^1];
                int boxId = Hash(label);
                var step = Boxes[boxId].FirstOrDefault(b => b.Label == label);
                if (step != null)
                    Boxes[boxId].Remove(step);
            }
            else if (seq.IndexOf('=') > 0)
            {
                var split = seq.Split("=");
                var step = new Step { Label = split[0], Focal = int.Parse(split[1]) };
                int boxId = Hash(step.Label);

                var existingStep = Boxes[boxId].FirstOrDefault(b => b.Label == step.Label);
                if (existingStep != null)
                {
                    var index = Boxes[boxId].IndexOf(existingStep);

                    if (index != -1)
                        Boxes[boxId][index] = step;
                }
                else
                    Boxes[boxId].Add(step);
            }
            else
                throw new ArgumentException($"{seq} is not -, not =");
        }

        for (var i = 0; i < Boxes.Length; i++)
        {
            var j = 0;
            foreach (var step in Boxes[i])
            {
                result += (1 + i) * (j + 1) * step.Focal;
                j++;
            }
        }
        return result;
    }

    int Hash(string label)
    {
        var current = 0;
        for (var i = 0; i < label.Length; i++)
        {
            int code = label[i];
            current += code;
            current *= 17;
            current %= 256;
        }
        return current;
    }
}
