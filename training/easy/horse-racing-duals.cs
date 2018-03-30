using System;
using System.Linq;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        // 1 < numHorses < 100000
        int numHorses = int.Parse(Console.ReadLine());

        // 0 < strength <= 10000000
        var horseStrengths = Enumerable.Range(0, numHorses)
            .Select(i => int.Parse(Console.ReadLine()))
            .OrderBy(s => s)
            .ToArray();
        
        var difference = horseStrengths
            .Zip(horseStrengths.Skip(1), (a, b) => b - a)
            .Min();

        Console.Error.WriteLine($"numHorses: {numHorses}");
        Console.WriteLine(difference);
    }
}
