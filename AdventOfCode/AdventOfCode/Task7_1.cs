﻿using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task7_1
{
    public class Card
    {
        public string? Hand;
        public int Number;
        public char Type;
        public int Bid;
    }

    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task7.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    public static long ProcessLines(string[] lines)
    {
        var resultList = new ArrayList();
        foreach (var line in lines)
        {
            var dict = new Dictionary<char, Card>();
            var hand = line.Split(' ')[0];
            var bid = line.Split(' ')[1];
            foreach (var c in hand)
            {
                if (dict.ContainsKey(c))
                {
                    dict[c].Number++;
                }
                else
                {
                    dict[c] = new Card()
                    {
                        Hand = hand,
                        Number = 1,
                        Type = c,
                        Bid = int.Parse(bid)
                    };
                }
            }
            resultList.Add(dict);
        }

        resultList.Sort(new listComparer());

        long result = 0;
        for (var i = 0; i < resultList.Count; i++)
        {
            var item = ((Dictionary<char, Card>)resultList[i]).Values.ToArray()[0];
            result += item.Bid * (i + 1);
        }
        return result;
    }

    public class listComparer : IComparer
    {
        int IComparer.Compare(Object a, Object b)
        {
            var dict1 = (Dictionary<char, Card>)a;
            var dict2 = (Dictionary<char, Card>)b;

            var list1 = dict1.ToList();
            var list2 = dict2.ToList();

            list1.Sort(delegate (KeyValuePair<char, Card> pair1,
                KeyValuePair<char, Card> pair2)
            {
                if (pair1.Value.Number > pair2.Value.Number)
                {
                    return -1;
                }
                else if (pair1.Value.Number < pair2.Value.Number)
                {
                    return 1;
                }
                else
                {
                    if (Weight(pair1.Value.Type) > Weight(pair2.Value.Type))
                    {
                        return -1;
                    }
                    else if (Weight(pair1.Value.Type) < Weight(pair2.Value.Type))
                    {
                        return 1;
                    }
                    else
                    {
                        var cardComparer = new CardComparer();
                        return cardComparer.Compare(pair1.Value, pair2.Value);
                    }
                }
            });

            list2.Sort(delegate (KeyValuePair<char, Card> pair1,
                KeyValuePair<char, Card> pair2)
            {
                if (pair1.Value.Number > pair2.Value.Number)
                {
                    return -1;
                }
                else if (pair1.Value.Number < pair2.Value.Number)
                {
                    return 1;
                }
                else
                {
                    if (Weight(pair1.Value.Type) > Weight(pair2.Value.Type))
                    {
                        return -1;
                    }
                    else if (Weight(pair1.Value.Type) < Weight(pair2.Value.Type))
                    {
                        return 1;
                    }
                    else
                    {
                        var cardComparer = new CardComparer();
                        return cardComparer.Compare(pair1.Value, pair2.Value);
                    }
                }
            });

            for (var i = 0; i < list1.Count && i < list2.Count; i++)
            {
                if (list1[i].Value.Number > 1 || list2[i].Value.Number > 1)
                {
                    if (list1[i].Value.Number == list2[i].Value.Number)
                    {
                        continue;
                    }
                    else
                    {
                        var res = list1[i].Value.Number - list2[i].Value.Number;
                        return res / Math.Abs(res);
                    }
                }
                else
                {
                    break;
                }
            }

            //for (var i = 0; i < list1.Count && i < list2.Count; i++)
            //{
            //    if (list1[i].Value.Number > 1 || list2[i].Value.Number > 1)
            //    {
            //        if (list1[i].Value.Number == list2[i].Value.Number)
            //        {
            //            if (Weight(list1[i].Value.Type) == Weight(list2[i].Value.Type))
            //            {
            //                //for (var j = 0; j < list1[i].Value.Hand.Length; j++)
            //                //{
            //                //    if (Weight(list1[i].Value.Hand[j]) != Weight(list1[i].Value.Hand[j]))
            //                //    {
            //                //        return Weight(list1[i].Value.Hand[j]) - Weight(list1[i].Value.Hand[j]);
            //                //    }
            //                //}
            //                continue;
            //            }
            //            else
            //            {
            //                var res = Weight(list1[i].Value.Type) - Weight(list2[i].Value.Type);
            //                return res / Math.Abs(res);
            //            }
            //        }
            //        else
            //        {
            //            var res = list1[i].Value.Number - list2[i].Value.Number;
            //            return res / Math.Abs(res);
            //        }
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            for (var j = 0; j < list1[0].Value.Hand.Length; j++)
            {
                if (Weight(list1[0].Value.Hand[j]) != Weight(list2[0].Value.Hand[j]))
                {
                    var res = Weight(list1[0].Value.Hand[j]) - Weight(list2[0].Value.Hand[j]);
                    return res / Math.Abs(res);
                }
            }

            return 0;
           
        }
    }

    public class CardComparer : Comparer<Card>
    {
        public override int Compare(Card x, Card y)
        {
            _ = x ?? throw new ArgumentNullException(nameof(x));
            _ = y ?? throw new ArgumentNullException(nameof(y));

            for (int i = 0; i < x.Hand.Length; i++)
            {
                if (Weight(x.Hand[i]) > Weight(y.Hand[i]))
                { 
                    return 1;
                }
                else if (Weight(x.Hand[i]) == Weight(y.Hand[i]))
                {
                    continue;
                }
                else 
                {
                    return -1;   
                }
            }
            return 0;
        }
    }

    static int Weight(char c) 
    {
        if (int.TryParse(c.ToString(), out int result))
        { 
            return result;
        }

        return c switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => throw new Exception("Wrong card")
        };
    }
}
