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
        string MESSAGE = Console.ReadLine();

        var binaryEnum = MESSAGE.ToCharArray()
            // Convert each char into their binary representation
            .Select(c => Convert.ToString(c, 2))
            // Pad each binary block with zeroes so they are always 7 bits long
            .Select(b => b.PadLeft(7, '0'))
            // Select each individual digit in the binary string
            .Select(b => b.ToCharArray())
            // Flatten to a single dimensional enumeration
            .SelectMany(d => d);

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        // Use stringbuilder for memory efficiency
        var sb = new StringBuilder();

        var lastDigit = ' ';
        foreach (var digit in binaryEnum)
        {
            // If the current digit is different than the last, add "0" for 1 or "00" for 0, seperated by spaces
            if (digit != lastDigit)
                sb.Append(digit == '1' ? " 0 " : " 00 ");

            // Since we are just counting the number of repeating digits, just always add a "0"
            sb.Append("0");
            lastDigit = digit;
        }

        // Trim to remove the extra space at the beginning
        Console.WriteLine(sb.ToString().Trim());
    }
}
