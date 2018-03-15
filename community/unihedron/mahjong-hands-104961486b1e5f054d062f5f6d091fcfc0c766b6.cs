using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
        var input = Console.ReadLine();

        var hand = GetTileset(input);
        var numPairs = GetNumberOfPairs(hand);
        var numSets = GetNumberOfSets(hand);
        var hasKokushiMusouHand = HasKokushiMusou(hand);

        Console.Error.WriteLine($"The hand: {GetTilesetString(hand)}");
        Console.Error.WriteLine($"Number of pairs: {numPairs}");
        Console.Error.WriteLine($"Number of sets: {numSets}");
        Console.Error.WriteLine($"Kokushi Musou: {hasKokushiMusouHand}");

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        var winCondition1 = numSets == 4 && (numPairs - numSets) == 1;
        var winCondition2 = numPairs == 7;
        var winCondition3 = hasKokushiMusouHand;

        if (winCondition1 || winCondition2 || winCondition3)
            Console.WriteLine("TRUE");
        else
            Console.WriteLine("FALSE");
    }

    static int GetNumberOfSets(IDictionary<char, int[]> tileset)
    {
        var numTriplets = GetNumberOfTriplets(tileset);

        // TODO: This algorithm isn't entirely correct yet
        var sets = tileset.Keys
            // The honor suitet can't be a set
            .Where(suite => suite != 'z')
            .Select(suite => tileset[suite]
                .Distinct()
                .Zip(tileset[suite].Distinct().Skip(1), (first, second) => new { first, second, res = (first + 1) == second})
                .Zip(tileset[suite].Distinct().Skip(2), (first, third) => new { first.first, first.second, third, res = first.res && (first.second + 1) == third })
            );
        
        return numTriplets + sets
            .SelectMany(set => set)
            .Where(set => set.res)
            .Count();
    }

    static int GetNumberOfTriplets(IDictionary<char, int[]> tileset)
    {
        return tileset.Keys
            // Flatten suit count
            .SelectMany(suit => tileset[suit]
                // Find triplets (or better) in each suit
                .GroupBy(tile => tile, (tile, group) => group.Count())
                .Where(groupCount => groupCount >= 3))
            .Count();
    }

    static int GetNumberOfPairs(IDictionary<char, int[]> tileset)
    {
        return tileset.Keys
            // Flatten suit count
            .SelectMany(suit => tileset[suit]
                // Find pairs (or better) in each suit
                .GroupBy(tile => tile, (tile, group) => group.Count())
                .Where(groupCount => groupCount >= 2))
            .Count();
    }

    static bool HasKokushiMusou(IDictionary<char, int[]> tileset)
    {
        Func<int, bool> isOneOrNine = tile => tile == 1 || tile == 9;

        var honorSuite = new[]{1,2,3,4,5,6,7};
        // Join the complete honor suite with the one in the tileset and get the amount of each tile
        var honorGrouping = honorSuite
            .GroupJoin(tileset['z'], tile => tile, tile => tile, (tile, innerTiles) => innerTiles.Count())
            .ToArray();

        return tileset['m'].All(isOneOrNine)
            && tileset['p'].All(isOneOrNine)
            && tileset['s'].All(isOneOrNine)
            // Has every tile in the honor suite
            && honorGrouping.Length == honorSuite.Length
            // Has a pair in the honor suite
            && honorGrouping.Any(count => count == 2);
    }

    static IDictionary<char, int[]> GetTileset(string input)
    {
        input = input.Replace(" ", string.Empty);

        var tiles = new Dictionary<char, List<int>>
        {
            { 'p', new List<int>() },   // Pins
            { 's', new List<int>() },   // Sous
            { 'm', new List<int>() },   // Mans
            { 'z', new List<int>() }    // Honor
        };

        var enumerator = input
            .ToCharArray()
            .GetEnumerator();
        
        var currentSuit = new List<int>();
        while (enumerator.MoveNext())
        {
            var currentChar = (char)enumerator.Current;
            if (char.IsDigit(currentChar))
                currentSuit.Add(currentChar - 48);
            else
            {
                tiles[currentChar].AddRange(currentSuit);
                currentSuit.Clear();
            }
        }

        return tiles.Keys
            //.Where(suit => tiles[suit].Any())
            .ToDictionary(suit => suit, suit => tiles[suit].OrderBy(val => val).ToArray());
    }

    static string GetTilesetString(IDictionary<char, int[]> tileset)
    {
        return string.Concat(tileset.Keys
            .Where(suit => tileset[suit].Any())
            .Select(suit => string.Concat(tileset[suit]) + suit));
    }
}
