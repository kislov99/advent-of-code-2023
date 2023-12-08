using System;
using System.Collections.Generic;

class Task8_3
{
    static long Gcd(long a, long b)
    {
        return b == 0 ? a : Gcd(b, a % b);
    }

    static long Lcm(long a, long b)
    {
        return a * b / Gcd(a, b);
    }

    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task8.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    public static long ProcessLines(string[] lines)
    {
        var instruction = lines[0];
        // Tree creation
        Dictionary<string, Dictionary<char, string>> tree = new Dictionary<string, Dictionary<char, string>>();
        List<string> startingNodes = new List<string>();
        for (var i = 2; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line == null || line == "")
            {
                break;
            }

            string[] parts = line.Split(" = (, )".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string node = parts[0].Trim();
            string left = parts[1].Trim();
            string right = parts[2].Trim();

            tree[node] = new Dictionary<char, string>
            {
                {'L', left},
                {'R', right}
            };

            if (node[2] == 'A')
            {
                startingNodes.Add(node);
            }
        }

        // Each starting node has a different number of steps to the end
        List<int> resultsList = new List<int>();
        foreach (var startingNode in startingNodes)
        {
            string currentNode = startingNode;
            int stepsToEnd = 0;

            while (currentNode[2] != 'Z')
            {
                int index = stepsToEnd % instruction.Length;
                char side = instruction[index];
                currentNode = tree[currentNode][side];
                stepsToEnd++;
            }

            resultsList.Add(stepsToEnd);
        }

        // Find the LCM of all the steps
        long result = 1;
        foreach (var v in resultsList)
        {
            result = Lcm(result, v);
        }

        return result;
    }
}