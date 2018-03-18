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
        // 0 < width < 30
        var width = int.Parse(Console.ReadLine());
        // 0 < height < 30
        var height = int.Parse(Console.ReadLine());
        // 0 < text.Length < 200
        var text = Console.ReadLine().ToUpper();

        // Read the ascii art design
        var lines = Enumerable.Range(0, height)
            .Select(row => Console.ReadLine())
            .ToArray();

        // Convert the design into discrete chars. A-Z makes 26 chars + a question mark
        var asciiLetters = Enumerable.Range(0, 27)
            .ToDictionary(n => (char)('A' + n), n => lines
                .Select(line => line.Substring(width * n, width))
                .ToArray());

        // Use the last char as fallback if the requested char is not in the design
        var fallbackAscii = asciiLetters.Values.Last();

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        // Get each line segment. Number of segments == ascii height
        var outputSegments = Enumerable.Range(0, height)
            // Concatenate the same row of each letter to form a line segment
            .Select(r => string.Concat(text.ToCharArray()
                .Select(c => 
                {
                    // Convert each char into an ascii letter
                    string[] ascii;
                    if (!asciiLetters.TryGetValue(c, out ascii))
                        ascii = fallbackAscii;
                    
                    // Get only the current row. This could probably be done more efficient
                    return ascii[r];
                })
                .ToArray()
            ))
            .ToArray();

        foreach (var c in outputSegments)
        {
            var segment = c;
            //Console.Error.WriteLine(segment);
            Console.WriteLine(segment);
        }
    }
}
