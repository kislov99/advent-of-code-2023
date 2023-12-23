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

public class Task19_2
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
        public int Min;
        public int Max;

        public override string ToString() => $"{Param}:{Min.ToString().PadLeft(4, '0')}-{Max.ToString().PadLeft(4, '0')}".PadLeft(15, ' '); 
    }

    class Parts
    {
        public string Id => string.Join(" | ", PartDict.Values.Select(x => x.ToString()));
        public string WorkflowName;
        public string Workflows;
        public Dictionary<char, Part> PartDict;
    }

    public long ProcessLines(string[] lines)
    {
        long result = 0;
        var i = 0;
        var workflows = new Dictionary<string, List<Rule>>();

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

        var ratings = new List<Parts>()
        {
            new Parts { PartDict =
                new Dictionary<char, Part>()
                {
                    { 'x', new Part { Param = 'x', Min = 1, Max = 4000 } },
                    { 'm', new Part { Param = 'm', Min = 1, Max = 4000 } },
                    { 'a', new Part { Param = 'a', Min = 1, Max = 4000 } },
                    { 's', new Part { Param = 's', Min = 1, Max = 4000 } },
                },
                WorkflowName = "in",
            }
        };

//        var resultingRaitings = new List<List<Rule>>();

        while (!ratings.All(x => x.WorkflowName == "A" || x.WorkflowName == "R"))
        {
            foreach (var parts in ratings.ToList())
            {
                var endOfWorkflow = false;
                do
                {
                    parts.Workflows += parts.WorkflowName + " > ";
                    if (parts.WorkflowName == "A" || parts.WorkflowName == "R")
                    {
                        break;
                    }
                    var workflow = workflows[parts.WorkflowName];

                    foreach (var rules in workflow)
                    {
                        if (rules.Condition == Condition.Final)
                        {
                            parts.WorkflowName = rules.WorkflowName;
                            if (rules.WorkflowName == "A" || rules.WorkflowName == "R")
                            {
                                endOfWorkflow = true;
                                break;
                            }
                            else
                            {
                                endOfWorkflow = true;
                                break;
                            }
                        }
                        else //if (parts.ContainsKey(rules.Param))
                        {
                            if (parts.PartDict[rules.Param].Min > parts.PartDict[rules.Param].Max)
                                throw new Exception("Min > Max");

                            if (rules.Condition == Condition.More)
                            {
                                if (parts.PartDict[rules.Param].Min > rules.Number)
                                {
                                    // whole range
                                    parts.WorkflowName = rules.WorkflowName;
                                }
                                else
                                {
                                    if (parts.PartDict[rules.Param].Max <= rules.Number)
                                    {
                                        // no intersaction
                                    }
                                    else
                                    {
                                        var newParts = new Dictionary<char, Part>();
                                        foreach (var part in parts.PartDict.Values)
                                        {
                                            Part newPart;
                                            if (part.Param == rules.Param)
                                                newPart = new Part { Param = part.Param, Min = rules.Number + 1, Max = part.Max};
                                            else
                                                newPart = new Part { Param = part.Param, Min = part.Min, Max = part.Max };
                                            newParts.Add(newPart.Param, newPart);
                                        }
                                        ratings.Add(new Parts { PartDict = newParts, WorkflowName = rules.WorkflowName, Workflows = parts.Workflows });
                                        parts.PartDict[rules.Param].Max = rules.Number;
                                    }
                                }
                            }
                            else if (rules.Condition == Condition.Less)
                            {
                                if (parts.PartDict[rules.Param].Max < rules.Number)
                                {
                                    // whole range
                                    parts.WorkflowName = rules.WorkflowName;
                                }
                                else
                                {
                                    if (parts.PartDict[rules.Param].Min >= rules.Number)
                                    {
                                        // no intersaction
                                    }
                                    else
                                    {
                                        var newParts = new Dictionary<char, Part>();
                                        foreach (var part in parts.PartDict.Values)
                                        {
                                            Part newPart;
                                            if (part.Param == rules.Param)
                                                newPart = new Part { Param = part.Param, Min = part.Min, Max = rules.Number - 1 };
                                            else
                                                newPart = new Part { Param = part.Param, Min = part.Min, Max = part.Max};
                                            newParts.Add(newPart.Param, newPart);
                                        }
                                        ratings.Add(new Parts { PartDict = newParts, WorkflowName = rules.WorkflowName, Workflows = parts.Workflows });
                                        parts.PartDict[rules.Param].Min = rules.Number;
                                    }
                                }
                            }
                        }
                    }
                } while (!endOfWorkflow);
            }
        }

        //in{ s < 1351:px,qqz}
        //           ^^^^^ 
        File.Delete("rating.txt");
        foreach (var rating in ratings)
        {
            if (rating.WorkflowName == "A")
            {
                File.AppendAllText("rating.txt", rating.Id.PadLeft(20, ' ') + $" - {rating.Workflows}{rating.WorkflowName}\r\n");
                long tmp = 1;
                foreach (var part in rating.PartDict)
                {
                    tmp *= part.Value.Max - part.Value.Min + 1;
                }
                result += tmp;
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
