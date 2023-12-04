using System;

namespace AdventOfCode;

public class Task1
{
    static string[] digits = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    public static void ProcessFile()
    {
        var lines = File.ReadAllLines("task1.txt");
        var sum = 0;

        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }

        Console.WriteLine(sum);
    }

    public static int ProcessLine(string input)
    {
        string result = string.Empty;

        var strPosition1 = -1;
        var strPosition2 = -1;
        var strNumber1 = "";
        var strNumber2 = "";
        var checkPosition = -1;

        var digit = 1;
        foreach (var digitStr in digits)
        {
            checkPosition = input.IndexOf(digitStr);
            if (checkPosition >= 0)
            {
                if (strPosition1 == -1 || strPosition1 > checkPosition)
                {
                    strPosition1 = checkPosition;
                    strNumber1 = digit.ToString();
                }
            }
            digit++;
        }

        if (strPosition1 != -1)
        {
            strPosition2 = strPosition1;
            strNumber2 = strNumber1;
            do
            {
                digit = 1;
                foreach (var digitStr in digits)
                {
                    checkPosition = input.Substring(strPosition2 + 1).IndexOf(digitStr);
                    if (checkPosition > 0)
                    {
                        strPosition2 = strPosition2 + checkPosition + 1;
                        strNumber2 = digit.ToString();
                        break;
                    }
                    digit++;
                }
            }
            while (checkPosition > 0);
        }


        if (strPosition2 == -1 && strPosition1 != -1)
        {
            strPosition2 = strPosition1;
            strNumber2 = strNumber1;
        }

        var position1 = -1;
        var digitNumber1 = string.Empty;
        foreach (var item in input)
        {
            position1++;
            var stringChar = item.ToString();
            if (int.TryParse(stringChar, out _))
            {
                if (string.IsNullOrEmpty(result))
                {
                    digitNumber1 = stringChar;
                    break;
                }
            }
        }

        if (strPosition1 != -1)
        {
            if (position1 < strPosition1)
            {
                result += digitNumber1;
            }
            else
            {
                result += strNumber1;
            }
        }
        else
        {
            result += digitNumber1;
        }


        int position2 = -1;
        var digitNumber2 = string.Empty;
        foreach (var item in input.Reverse())
        {
            position2++;
            var stringChar = item.ToString();
            if (int.TryParse(stringChar, out _))
            {
                digitNumber2+= stringChar;
                break;
            }
        }

        if (strPosition2 != -1)
        {
            if (input.Length - position2 - 1 > strPosition2)
            {
                result += digitNumber2;
            }
            else
            {
                result += strNumber2;
            }
        }
        else 
        { 
            result += digitNumber2; 
        }

        return int.Parse(result);
    }
}