using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task12_2
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task12.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Combo
    {
        public string Pattern;
        public List<int> Questions;
        public List<int> Numbers;
    }

    class Pair
    { 
        public int Index;
        public bool IsDamaged;
    }

    public long ProcessLines(string[] lines)
    {
        long result = 0;
        var list = new List<Combo>();
        for (var y = 0; y<lines.Length; y++)
        {
            var split = lines[y].Split(' ');
            var combo = new Combo
            {
                Pattern = split[0],
                Numbers = split[1].Split(',').Select(x => int.Parse(x)).ToList()
            };

            for (var i = 0; i < 4; i++)
            {
                combo.Pattern = combo.Pattern + "?" + split[0];
                combo.Numbers.AddRange(split[1].Split(',').Select(x => int.Parse(x)).ToList());
            }

            list.Add(combo);
        }

        for (var i =0; i < list.Count; i++) 
        {
            var combo = list[i];
            GetCount(combo.Pattern, combo.Numbers, ref result, false);
            Console.WriteLine(i);
        }

        return result;
    }

    void GetCount(string pattern, List<int> numbers, ref long result, bool attached)
    {
        if (pattern == "")
        {
            if (numbers.Count == 0)
                result++;
            else
                return;
        }

        if (pattern.StartsWith('.'))
        {
            if (!attached)
                GetCount(pattern.Substring(1), numbers, ref result, false);
            return;            
        }

        if (pattern.StartsWith('#'))
        {
            if (numbers.Count > 0 && numbers[0] > 0)
            {
                var newNumbers = new List<int>();
                newNumbers.AddRange(numbers);
                newNumbers[0]--;
                if (newNumbers[0] == 0)
                { 
                    newNumbers.RemoveAt(0);

                    if (pattern.Length > 1)
                    {
                        if ((pattern[1] == '.' || pattern[1] == '?'))
                            GetCount(pattern.Substring(2), newNumbers, ref result, false);
                        else
                            return;
                    }
                    else
                        GetCount(pattern.Substring(1), newNumbers, ref result, false);
                }
                else
                    GetCount(pattern.Substring(1), newNumbers, ref result, true);
            }
            else
                return;
        }

        if (pattern.StartsWith('?'))
        {
            var oldResult = result;
            var hash = "#" + pattern.Substring(1) + string.Join(',', numbers.Select(x => x.ToString()));
            
            if (cache.ContainsKey(hash))
                result += cache[hash];
            else
            {
                GetCount("#" + pattern.Substring(1), numbers, ref result, attached);

                if (!cache.ContainsKey(hash))
                    cache.Add(hash, result - oldResult);
            }
    
            if (!attached)
            {
                GetCount("." + pattern.Substring(1), numbers, ref result, attached);
            }
        }
    }

    Dictionary<string, long> cache = new Dictionary<string, long>();
}

