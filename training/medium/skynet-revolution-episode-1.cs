using System;
using System.Linq;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {
        var inputs = Console.ReadLine().Split(' ');
        // 2 <= numNodes <= 500 - the total number of nodes in the level, including the gateways
        var numNodes = int.Parse(inputs[0]);
        // 1 <= numLinks <= 1000 - the number of links between nodes
        var numLinks = int.Parse(inputs[1]);
        // 1 <= numGateways <= 20 - the number of exit gateways
        var numGateways = int.Parse(inputs[2]);

        var links = Enumerable.Range(0, numLinks)
            .Select(l => Console.ReadLine().Split(' '))
            .Select(l => new Tuple<int, int>(int.Parse(l[0]), int.Parse(l[1])))
            .ToList();

        var gateways = Enumerable.Range(0, numGateways)
            .Select(e => int.Parse(Console.ReadLine()))
            .ToArray();

        // game loop
        while (true)
        {
            // 0 <= agentIndex <= numNodes - The index of the node on which the Skynet agent is positioned this turn
            var agentIndex = int.Parse(Console.ReadLine());

            var link = links
                // Filter only links connected to agents current node
                .Where(l => l.Item1 == agentIndex || l.Item2 == agentIndex)
                // Prioritise links that connect to a gateway
                .OrderByDescending(l => gateways.Contains(l.Item1) || gateways.Contains(l.Item2))
                .FirstOrDefault();

            links.Remove(link);

            Console.WriteLine($"{link.Item1} {link.Item2}");
        }
    }
}
