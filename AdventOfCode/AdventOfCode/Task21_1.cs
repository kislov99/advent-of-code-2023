using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace AdventOfCode;

public class Task21_1
{
	public void ProcessFile()
	{
		var lines = File.ReadAllLines("task21.txt");

		Console.WriteLine(ProcessLines(lines));
	}


	char[,] Matrix;

	public long ProcessLines(string[] lines)
	{
		Matrix = new char[lines.Length, lines[0].Length];
		var start = new Coord();
		var result = 0;
		for (var y = 0; y < lines.Length; y++)
		{
			for (var x = 0; x < lines[y].Length; x++)
			{
				if (lines[y][x] == 'S')
				{
					start.X = x;
					start.Y = y;
				}
				Matrix[y, x] = lines[y][x];
			}
		}

		HashSet.Add(start);
		Go();

		return Matrix.Flatten().Count(x => x == 'O');
	}

	HashSet<Coord> HashSet = new HashSet<Coord>();
	List<(int y, int x)> Deltas = new List<(int y, int x)>
	{
		{ (0, 1) },
		{ (0, -1) },
		{ (1, 0) },
		{ (-1, 0) },
	};

	void Go()
	{
		for (var i = 0; i < 64; i++)
		{
			var nextSteps = new HashSet<Coord>();
			foreach (var coord in HashSet)
			{
				Matrix[coord.Y, coord.X] = '.';
				foreach (var (y1, x1) in Deltas)
				{
					var newY = coord.Y + y1;
					var newX = coord.X + x1;
					if (newY >= 0 && newY < Matrix.GetLength(0) &&
						newX >= 0 && newX < Matrix.GetLength(1) &&
						Matrix[newY, newX] != '#' &&
						Matrix[newY, newX] != 'O' &&
					 Matrix[newY, newX] != 'S')
					{
						nextSteps.Add(new Coord { X = newX, Y = newY });
						Matrix[newY, newX] = 'O';
					}
				}
			}
			HashSet = nextSteps;
			PrintMatrix("matrix.txt", Matrix);
		}
	}

	static void PrintMatrix(string filename, char[,] matrix)
	{
		File.Delete(filename);
		for (var y = 0; y < matrix.GetLength(0); y++)
		{
			StringBuilder sb = new StringBuilder();
			for (var x = 0; x < matrix.GetLength(1); x++)
			{
				sb.Append(matrix[y, x]);
			}
			File.AppendAllText(filename, sb.ToString() + Environment.NewLine);
		}
	}
}
