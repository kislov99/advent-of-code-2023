using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Task17
{
    public void ProcessFile()
    {
        var lines = File.ReadAllLines("task17.txt");

        Console.WriteLine(ProcessLines(lines));
    }

    class Item
    {
        public int Heat;
        public int MinHeat = int.MaxValue;
        public bool Visited;
    }
    
    record struct Node {
        public int Y; 
        public int X;
        public Direction Direction; 
        public int StraightLine;
    }

    static Item[,] Matrix;
    static Dictionary<Node, int> Visited = new Dictionary<Node, int>();


    PriorityQueue<Node, int> PriorityQueue = new PriorityQueue<Node, int>();

    class Path
    {
        public Node Node = new Node();
        public int Heat = 0;
    }

    public long ProcessLines(string[] lines)
    {
        var result = 0;
        Matrix = new Item[lines[0].Length, lines.Length];
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                Matrix[y, x] = new Item { Heat = int.Parse(lines[y][x].ToString()) };
            }
        }

        var init = new Node
        {
            Y = 0,
            X = 0,
            Direction = Direction.East,
            StraightLine = 0
        };

        var state = init;

        PriorityQueue.Enqueue(state, 0);
        Visited.Add(init, 0);

        while (PriorityQueue.Count > 0) 
        {
            state = PriorityQueue.Dequeue();
            if (state.Y == Matrix.GetLength(0) - 1 && state.X == Matrix.GetLength(1) - 1 && state.StraightLine >= 4)
                return Visited[state];

            Check(state);
        }

        return 0;
    }

    Dictionary<Direction, (Direction d1, Direction d2)> DirectionMap = new Dictionary<Direction, (Direction d1, Direction d2)>
    {
        { Direction.East, (Direction.South, Direction.North) },
        { Direction.West, (Direction.South, Direction.North) },
        { Direction.South, (Direction.West, Direction.East) },
        { Direction.North, (Direction.West, Direction.East) },
    };

    Dictionary<Direction, (int y, int x)> CoordDeltaMap = new Dictionary<Direction, (int y, int x)>
    {
        { Direction.East, (0, 1) },
        { Direction.West, (0, -1) },
        { Direction.South, (1, 0) },
        { Direction.North, (-1, 0) },
    };
    
    void Check(Node node)
    {
        var y = node.Y;
        var x = node.X;

        var (d2, d3) = DirectionMap[node.Direction];
        var (y1, x1) = CoordDeltaMap[node.Direction];
        var (y2, x2) = CoordDeltaMap[d2];
        var (y3, x3) = CoordDeltaMap[d3];

        Path newPath1 = null;
        Node node1 = new Node
        {
            X = x + x1,
            Y = y + y1,
            Direction = node.Direction,
            StraightLine = node.StraightLine + 1,
        };

        if (x + x1 >= 0 && y + y1 >= 0 &&
            y + y1 < Matrix.GetLength(0) &&
            x + x1 < Matrix.GetLength(1) &&
            (!Visited.ContainsKey(node1) || 
            Visited[node1] > Visited[node] + Matrix[y + y1, x + x1].Heat) &&
            node.StraightLine < 10)
        {
            if (!Visited.ContainsKey(node1))
               Visited.Add(node1, Visited[node] + Matrix[y + y1, x + x1].Heat);
            else
                Visited[node1] = Visited[node] + Matrix[y + y1, x + x1].Heat;
            PriorityQueue.Enqueue(node1, Visited[node1]);
        }

        Path newPath2 = null;
        Node node2 = new Node
        {
            X = x + x2,
            Y = y + y2,
            Direction = d2,
            StraightLine = 1,
        };

        if (x + x2 >= 0 && y + y2 >= 0 &&
            y + y2 < Matrix.GetLength(0) &&
            x + x2 < Matrix.GetLength(1) &&
            (!Visited.ContainsKey(node2) ||
            Visited[node2] > Visited[node] + Matrix[y + y2, x + x2].Heat) &&
            node.StraightLine >= 4)
        {
            if (!Visited.ContainsKey(node2))
                Visited.Add(node2, Visited[node] + Matrix[y + y2, x + x2].Heat);
            else
                Visited[node2] = Visited[node] + Matrix[y + y2, x + x2].Heat;
            PriorityQueue.Enqueue(node2, Visited[node2]);
        }

        Path newPath3 = null;
        Node node3 = new Node
        {
            X = x + x3,
            Y = y + y3,
            Direction = d3,
            StraightLine = 1,
        };
        if (x + x3 >= 0 && y + y3 >= 0 &&
            y + y3 < Matrix.GetLength(0) &&
            x + x3 < Matrix.GetLength(1) &&
            (!Visited.ContainsKey(node3) ||
            Visited[node3] > Visited[node] + Matrix[y + y3, x + x3].Heat) &&
            node.StraightLine >= 4)
        {
            if (!Visited.ContainsKey(node3))
                Visited.Add(node3, Visited[node] + Matrix[y + y3, x + x3].Heat);
            else
                Visited[node3] = Visited[node] + Matrix[y + y3, x + x3].Heat;
            PriorityQueue.Enqueue(node3, Visited[node3]);
        }
    }
}
