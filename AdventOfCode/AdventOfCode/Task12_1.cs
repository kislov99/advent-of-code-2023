using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task12_1
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task12.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Combo
    {
        public StringBuilder Pattern;
        public List<int> Questions;
        public string Numbers;
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
                Pattern = new StringBuilder(split[0]),
                Numbers = split[1]
            };

            var index = -1;
            combo.Questions = new List<int>();
            do
            {
                index = combo.Pattern.ToString().IndexOf('?', index + 1);
                if (index == -1)
                    break;
                combo.Questions.Add(index);
            }
            while (index >= 0);
            list.Add(combo); 
        }

        foreach (var combo in list) 
        {
            var resultList = new Pair[combo.Questions.Count];
            bigList = new List<Pair[]>();
            Permutations(0, combo.Questions, ref resultList, true);
            Permutations(0, combo.Questions, ref resultList, false);
            
            var temp = new StringBuilder(combo.Pattern.ToString());
            foreach (var array in bigList)
            {
                temp = new StringBuilder(combo.Pattern.ToString());
                foreach (var item in array)
                {
                    if (item.IsDamaged)
                        temp[item.Index] = '#';
                    else
                        temp[item.Index] = '.';
                }
                if (Check(new Combo { Pattern = temp, Numbers = combo.Numbers }))
                    result++;
            }
        }
        return result;
    }

    List<Pair[]> bigList;
    void Permutations(int index, List<int> questions, ref Pair[] resultList, bool add)
    {
        if (index < questions.Count)
        {
            if (add)
            {
                resultList[index] = new Pair { Index = questions[index], IsDamaged = true };
            }
            else
            {
                resultList[index] = new Pair { Index = questions[index], IsDamaged = false };
            }
            Permutations(index + 1, questions, ref resultList, true);
            if (index < questions.Count - 1)
                Permutations(index + 1, questions, ref resultList, false);
            return;
        }

        bigList.Add(resultList);
        var tmp = new Pair[questions.Count];
        for(var i = 0; i < resultList.Length; i++) 
        {
            tmp[i] = new Pair { Index = resultList[i].Index, IsDamaged = resultList[i].IsDamaged };
        }
        resultList = tmp;
    }


    bool Check(Combo combo)
    {
        var numbers = new List<int>();
        var number = 0;
        foreach (var c in combo.Pattern.ToString())
        {
            if (c == '.')
            {
                if (number > 0)
                    numbers.Add(number);
                number = 0;

            }
            else if (c == '#')
            {
                number++;
            }
            else if (c == '?')
                throw new ArgumentException("? is not supposed to be here");
        }
        if (number > 0)
            numbers.Add(number);

        if (string.Join(',', numbers) == combo.Numbers)
            return true;
        else
            return false;
    }
}

