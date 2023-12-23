using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data;
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

public class Task19_1
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task19.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Rule
    {
        public char Param;
        public Condition Condition;
        public int Number;
        public string WorkflowName;
    }

    class Part
    {
        public char Param;
        public int Number;
    }



    public long ProcessLines(string[] lines)
    {
        var result = 0;
        var i = 0;
        var workflows = new Dictionary<string, List<Rule>>();
        var ratings = new List<Dictionary<char, int>>();

        while (lines[i] != "")
        {
            var splitWorkflow = lines[i].Split('{');
            var splitRules = splitWorkflow[1].Split(',');
            var rules = new List<Rule>();
            foreach (var ruleRaw in splitRules)
            {
                var rule = ParseRule(ruleRaw);
                rules.Add(rule);
            }
            workflows.Add(splitWorkflow[0], rules);
            i++;
        }

        i++;

        while (i < lines.Length)
        {
            var splitParts = lines[i][1..^1].Split(',');
            var parts = new Dictionary<char, int>();
            foreach (var splitPart in splitParts)
            {
                var part = splitPart.Split('=');
                parts.Add(part[0][0], int.Parse(part[1]));
            }
            ratings.Add(parts);
            i++;
        }

        var resultingRaitings = new List<Dictionary<char, int>>();

        foreach (var parts in ratings)
        {
            var nextWorkflow = "in";
            var end = false;
            do
            {
                if (nextWorkflow == "A")
                {
                    resultingRaitings.Add(parts);
                    break;
                }
                else if (nextWorkflow == "R")
                {
                    break;
                }
                var workflow = workflows[nextWorkflow];

                foreach (var rules in workflow)
                {
                    if (rules.Condition == Condition.Final)
                    {
                        if (rules.WorkflowName == "A")
                        {
                            resultingRaitings.Add(parts);
                            end = true;
                            break;
                        }
                        else if (rules.WorkflowName == "R")
                        {
                            end = true;
                            break;
                        }
                        else
                        {
                            nextWorkflow = rules.WorkflowName;
                            break;
                        }
                    }
                    else if (parts.ContainsKey(rules.Param))
                    {
                        if (rules.Condition == Condition.More)
                        {
                            if (parts[rules.Param] > rules.Number)
                            {
                                nextWorkflow = rules.WorkflowName;
                                break;
                            }
                        }
                        else if (rules.Condition == Condition.Less)
                        {
                            if (parts[rules.Param] < rules.Number)
                            {
                                nextWorkflow = rules.WorkflowName;
                                break;
                            }
                        }
                    }
                    else throw new Exception($"unknown param {rules.Param}");
                }
            } while (!end);
        }

        foreach (var rating in resultingRaitings)
        {
            foreach (var part in rating)
            {
                result += part.Value; 
            }
        }

        return result;
    }

    enum Condition
    {
        More,
        Less,
        Final
    }

    Rule ParseRule(string ruleRaw)
    {
        var rule = new Rule();
        if (ruleRaw.IndexOf(':') > 0)
        {
            var splitRule = ruleRaw.Split(':');
            rule.WorkflowName = splitRule[1];
            if (splitRule[0].IndexOf('<') > 0)
            {
                var condition = splitRule[0].Split('<');
                rule.Condition = Condition.Less;
                rule.Number = int.Parse(condition[1]);
                rule.Param = condition[0][0];
            }
            else
            {
                var condition = splitRule[0].Split('>');
                rule.Condition = Condition.More;
                rule.Number = int.Parse(condition[1]);
                rule.Param = condition[0][0];
            }
        }
        else 
        {
            rule.WorkflowName = ruleRaw[..^1];
            rule.Condition = Condition.Final;
        }

        return rule;
    }
}
