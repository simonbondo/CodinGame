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
        // Number of elements which make up the association table.
        // 0 < N < 10000
        int N = int.Parse(Console.ReadLine());
        // Number Q of file names to be analyzed.
        // 0 < Q < 10000
        int Q = int.Parse(Console.ReadLine());

        var mimeMap = Enumerable.Range(0, N)
            .Select(n => Console.ReadLine().Split(' '))
            .ToDictionary(input => input[0].ToLower(), input => input[1]);

        for (int i = 0; i < Q; i++)
        {
            string FNAME = Console.ReadLine();

            // Get extension from input filename
            var extIndex = FNAME.LastIndexOf('.');
            var ext = extIndex >= 0
                ? FNAME.Substring(extIndex + 1).ToLower()
                : string.Empty;

            // Write mime type from map or UNKNOWN if not found
            mimeMap.TryGetValue(ext, out var mimeType);
            Console.WriteLine(mimeType ?? "UNKNOWN");
        }
    }
}
