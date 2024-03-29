using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
        string[] inputs = Console.ReadLine().Split(' ');

        int temp = 0;
        if (n != 0 && inputs.Length > 0)
        {
            temp = inputs
                .Select(int.Parse)
                .OrderBy(Math.Abs)
                .ThenByDescending(t => t)
                .First();
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine($"{temp}");
    }
}
