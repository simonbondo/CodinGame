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
    static int Width;
    static int Height;

    static void Main(string[] args)
    {
        var folds = int.Parse(Console.ReadLine());
        var inputs = Console.ReadLine().Split(' ');
        Width = int.Parse(inputs[0]);
        Height = int.Parse(inputs[1]);

        Console.Error.WriteLine($"Folds:{folds}, Size: {Width}x{Height}");

        var sheet = new bool[Height, Width];
        for (int row = 0; row < Height; row++)
        {
            var line = Console.ReadLine();
            for (int col = 0; col < Width; col++)
            {
                sheet[row, col] = line[col] == '#';
            }
        }

        for (int i = 0; i < folds; i++)
        {
            sheet = Unfold(sheet);
        }

        var areas = 0;
        for (int r = 0; r < Height; r++)
        {
            for (int c = 0; c < Width; c++)
            {
                if (sheet[r, c] == true)
                {
                    if (GetAreaSizeAndBlank(sheet, r, c) > 0)
                        areas++;
                }
            }
        }
 
        Console.WriteLine(areas);
    }

    public static int GetAreaSizeAndBlank(bool[,] sheet, int r, int c)
    {
        if (c < 0 || c >= Width || r < 0 || r >= Height || sheet[r, c] == false)
            return 0;
        
        sheet[r, c] = false;
        return 1 +
            GetAreaSizeAndBlank(sheet, r, c - 1) +
            GetAreaSizeAndBlank(sheet, r, c + 1) +
            GetAreaSizeAndBlank(sheet, r - 1, c) +
            GetAreaSizeAndBlank(sheet, r + 1, c);
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

    static bool[,] Unfold(bool[,] sheet)
    {
        var newHeight = Height * 2;
        var newWidth = Width * 2;
        // WTF... out of memory ?!
        var unfolded = new bool[newHeight, newWidth];

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                var val = sheet[row, col];
                unfolded[Height + row, Width + col] = val;
                unfolded[Height - row - 1, Width + col] = val;
                unfolded[Height + row, Width - col - 1] = val;
                unfolded[Height - row - 1, Width - col - 1] = val;
            }
        }
        Height = newHeight;
        Width = newWidth;
        return unfolded;
    }
}
