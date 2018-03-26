using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        var numFolds = int.Parse(Console.ReadLine());
        var inputs = Console.ReadLine().Split(' ');
        var width = int.Parse(inputs[0]);
        var height = int.Parse(inputs[1]);

        var pattern = Enumerable.Range(0, height)
            .Select(n => Console.ReadLine())
            .ToArray();

        Console.Error.WriteLine($"Folds:{numFolds}, Size: {width}x{height}");

        PrintPattern(pattern);

        /*
        // Get inital areas
        string[] patternCopy = new string[height];
        Array.Copy(pattern, patternCopy, height);
        var initalAreas = DoPhysicalSolution(patternCopy, 0);
        Console.Error.WriteLine($"Initial areas: {initalAreas}");
        */

        int solution = 0;

        // Actually unfold and detect areas
        // Gets slow and memory constrainted with high NumFolds or large Width/Height
        solution = DoPhysicalSolution(pattern, numFolds);

        // Calculate number of areas based on a ruleset
        // After all initial pieces has been classified, it should be very fast to calculate how pieces mutate and multiply
        //solution = DoRuleBasedSolution(pattern, numFolds); 

        Console.WriteLine(solution);
    }

    public static int DoRuleBasedSolution(string[] pattern, int numFolds)
    {
        var patterns = new[] {                                                          // After first fold:
            // Corner points
            new PatternInfo { Type = "NW", Pattern = new[] { "#..", "...", "..." } },   // --> 1Mi
            new PatternInfo { Type = "NE", Pattern = new[] { "..#", "...", "..." } },   // --> 1WC + 1EC
            new PatternInfo { Type = "SW", Pattern = new[] { "...", "...", "#.." } },   // --> 1NC + 1SC
            new PatternInfo { Type = "SE", Pattern = new[] { "...", "...", "..#" } },   // --> 1NW + 1NE + 1SW + 1SE
            // Edge points
            new PatternInfo { Type = "NC", Pattern = new[] { ".#.", "...", "..." } },   // --> 2Mi
            new PatternInfo { Type = "WC", Pattern = new[] { "...", "#..", "..." } },   // --> 2Mi
            new PatternInfo { Type = "EC", Pattern = new[] { "...", "..#", "..." } },   // --> 2WC + 2EC
            new PatternInfo { Type = "SC", Pattern = new[] { "...", "...", ".#." } },   // --> 2NC + 2SC
            // Middle point
            new PatternInfo { Type = "Mi", Pattern = new[] { "...", ".#.", "..." } },   // --> 4Mi
            // Bar patterns
            new PatternInfo { Type = "No", Pattern = new[] { "###", "...", "..." } },   // --> 1Ho
            new PatternInfo { Type = "We", Pattern = new[] { "#..", "#..", "#.." } },   // --> 1Ve
            new PatternInfo { Type = "Ea", Pattern = new[] { "..#", "..#", "..#" } },   // --> 1Ea + 1We
            new PatternInfo { Type = "So", Pattern = new[] { "...", "...", "###" } },   // --> 1No + 1So
            new PatternInfo { Type = "Ho", Pattern = new[] { "...", "###", "..." } },   // --> 2Ho
            new PatternInfo { Type = "Ve", Pattern = new[] { ".#.", ".#.", ".#." } },   // --> 2Ve
            // All Edges
            new PatternInfo { Type = "Al", Pattern = new[] { ".#.", "###", ".#." } },   // --> 1Al

            // Test patterns
            new PatternInfo { Type = "T1", Pattern = new[] { "...", ".##", ".#." } },   // --> 4??
            new PatternInfo { Type = "T2", Pattern = new[] {
                ".#.......#.....#...####",
                "##..#.....####.#.......",
                "#...###....#...#.###.##",
                "###........#...#....#.."
            }}, // Random pattern
        };

        /*
        After second fold:
        NW:  4   1Mi                        -->  4Mi
        NE:  6   1WC +  1EC                 -->  2WC +  2EC +  2Mi
        SW:  6   1NC +  1SC                 -->  2NC +  2SC +  2Mi
        SE:  9   1NW +  1NE +  1SW +  1SE   -->  1NW +  1NE +  1SW +  1SE +  1NC +  1WC +  1EC +  1SC +  1Mi

        NC:  8   2Mi        -->  8Mi
        WC:  8   2Mi        -->  8Mi
        EC: 12   2WC +  2EC -->  4WC +  4EC +  4Mi
        SC: 12   2NC +  2SC -->  4NC +  4SC +  4Mi

        Mi: 16   4Mi    --> 16Mi
        
        No:  2   1Ho        -->  2Ho
        We:  2   1Ve        -->  2Ve
        Ea:  3   1We +  1Ea -->  1We +  1Ea +  1Ve
        So:  3   1No +  1So -->  1No +  1So +  1Ho
        Ho:  4   2Ho        -->  4Ho
        Ve:  4   2Ve        -->  4Ve
        AE:  1   1AE        -->  1AE
        
        T1:  9   4??    -->  9??
        */
        
        /*
        After third fold:
        NW: 16   4Mi                                                            --> 16Mi
        NE: 20   2Mi +  2EC +  2WC                                              -->  8Mi +  4Mi +  4EC +  4WC
        SW: 20   2Mi +  2NC +  2SC                                              -->  8Mi +  4Mi +  4NC +  4SC
        SE: 25   1NW +  1NC +  1NE +  1EC +  1Mi +  1WC +  1SW +  1SC +  1SE    -->  1NW +  3NC +  1NE +  3EC +  9Mi +  3WC +  1SW +  3SC +  1SE

        NC: 32   8Mi                --> 32Mi
        WC: 40   4Mi +  4EC +  4WC  --> 16Mi +  8Mi +  8EC +  8WC
        EC: 32   8Mi                --> 32Mi
        SC: 40   4Mi +  4NC +  4SC  --> 16Mi +  8Mi +  8NC +  8SC

        Mi: 64  16Mi    --> 64Mi
        */

        foreach (var patternInfo in patterns)
        {
            for (int i = 0; i < numFolds; i++)
            {
                patternInfo.Pattern = UnfoldSheet(patternInfo.Pattern);
            }
            patternInfo.Areas = GetNumAreas(patternInfo.Pattern);
        }

        return 0;
    }

    static int GetNumAreas(string[] pattern)
    {
        var numAreas = 0;
        for (int row = 0; row < pattern.Length; row++)
        {
            var cols = pattern[row].Length;
            for (int col = 0; col < cols; col++)
            {
                if (FloodFill(pattern, row, col, '#', (numAreas + 1).ToString().ToCharArray().Last()) > 0)
                    numAreas++;
            }
        }
        return numAreas;
    }

    public static int FloodFill(string[] pattern, int row, int col, char target, char replacement)
    {
        if (target == replacement)
            return 0;
        if (row < 0 || row >= pattern.Length || col < 0 || col >= pattern[row].Length)
            return 0;

        var node = pattern[row][col];
        if (node != target)
            return 0;

        pattern[row] = $"{pattern[row].Remove(col)}{replacement}{pattern[row].Substring(col + 1)}";
        // Repeat for cell to left + up + right + down
        return 1 +
            FloodFill(pattern, row, col - 1, target, replacement) +
            FloodFill(pattern, row - 1, col, target, replacement) +
            FloodFill(pattern, row, col + 1, target, replacement) +
            FloodFill(pattern, row + 1, col, target, replacement);
    }

    public static int DoPhysicalSolution(string[] pattern, int numFolds)
    {
        var height = pattern.Length;
        var width = pattern[0].Length;
        var sheet = new bool[height, width];
        for (int row = 0; row < height; row++)
        {
            var line = pattern[row];
            for (int col = 0; col < width; col++)
            {
                sheet[row, col] = line[col] == '#';
            }
        }

        for (int i = 0; i < numFolds; i++)
        {
            sheet = Unfold(sheet, height, width);
            height *= 2;
            width *= 2;
        }

        var minAreaSize = 1;
        var numAreas = 0;
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                if (sheet[r, c] == true)
                {
                    if (GetAreaSizeAndFill(sheet, height, width, r, c) >= minAreaSize)
                        numAreas++;
                }
            }
        }
 
        return numAreas;
    }

    public static int GetAreaSizeAndFill(bool[,] sheet, int height, int width, int r, int c)
    {
        if (r < 0 || r >= height || c < 0 || c >= width || sheet[r, c] == false)
            return 0;
        
        sheet[r, c] = false;
        return 1 +
            GetAreaSizeAndFill(sheet, height, width, r, c - 1) +
            GetAreaSizeAndFill(sheet, height, width, r, c + 1) +
            GetAreaSizeAndFill(sheet, height, width, r - 1, c) +
            GetAreaSizeAndFill(sheet, height, width, r + 1, c);
    }
    
    static string[] UnfoldSheet(string[] sheet)
    {
        var h = sheet.Length;
        var w = sheet[0].Length;
        // WTF... out of memory ?!
        var unfoldedSheet = new string[h * 2];
        
        for (var r = 0; r < h; r++)
        {
            var row = sheet[r];
            var reverseRow = new string(row.ToCharArray().Reverse().ToArray());
            var newRow = reverseRow + row;
            
            unfoldedSheet[h + r] = newRow;
            unfoldedSheet[h - r - 1] = newRow;
        }
        
        return unfoldedSheet;
    }

    static bool[,] Unfold(bool[,] sheet, int height, int width)
    {
        var newHeight = height * 2;
        var newWidth = width * 2;
        // WTF... out of memory ?!
        var unfolded = new bool[newHeight, newWidth];

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                var val = sheet[row, col];
                unfolded[height + row, width + col] = val;
                unfolded[height - row - 1, width + col] = val;
                unfolded[height + row, width - col - 1] = val;
                unfolded[height - row - 1, width - col - 1] = val;
            }
        }
        return unfolded;
    }

    static void PrintPattern(string[] pattern)
    {
        var rowDigits = pattern.Length.ToString().Length;
        var rowFormat = new string('0', rowDigits);
        for (int r = 0; r < pattern.Length; r++)
        {
            Console.Error.WriteLine($"{r.ToString(rowFormat)}: {pattern[r]}");
        }
    }
}

public class PatternInfo
{
    public string Type { get; set; }
    public string[] Pattern { get; set; }
    public int Areas { get; set; }
}
