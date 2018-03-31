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

        // 0 <= node1, node2 < numNodes - node1 and node2 defines a link between these nodes
        var links = Enumerable.Range(0, numLinks)
            .Select(l => Console.ReadLine())
            .ToArray();

        // 0 <= gateway <= numNodes - the index of a gateway node
        var gateways = Enumerable.Range(0, numGateways)
            .Select(e => int.Parse(Console.ReadLine()))
            .ToArray();

        var network = SkynetNetwork.Create(numNodes, links, gateways);

        // game loop
        while (true)
        {
            // 0 <= agentIndex <= numNodes - The index of the node on which the Skynet agent is positioned this turn
            var agentIndex = int.Parse(Console.ReadLine());

            var gatewayLink = network.FindClosestGatewayLink(agentIndex);
            network.SeverLink(gatewayLink.Item1, gatewayLink.Item2);

            Console.WriteLine($"{gatewayLink.Item1} {gatewayLink.Item2}");
        }
    }
}

public class SkynetNetwork
{
    public NetworkNode[] NetworkNodes { get; set; }

    public NetworkNode this[int index] => this.NetworkNodes.SingleOrDefault(n => n.Id == index);

    public void CreateLink(int nodeIndex1, int nodeIndex2)
    {
        if (nodeIndex1 == nodeIndex2)
            return;

        var node1 = this[nodeIndex1];
        var node2 = this[nodeIndex2];

        if (node1 == null || node2 == null)
            return;

        node1.Links.Add(node2);
        node2.Links.Add(node1);
    }

    public void SeverLink(int nodeIndex1, int nodeIndex2)
    {
        if (nodeIndex1 == nodeIndex2)
            return;

        var node1 = this[nodeIndex1];
        var node2 = this[nodeIndex2];

        if (node1 == null || node2 == null)
            return;

        node1.Links.Remove(node2);
        node2.Links.Remove(node1);
    }

    public Tuple<int, int> FindClosestGatewayLink(int startIndex)
    {
        // Find closest gateway using breadth first search
        var visitedNodes = new List<NetworkNode>();
        var nodeQueue = new Queue<NetworkNode>();

        Func<NetworkNode, bool> unvisited = node => !visitedNodes.Contains(node);

        nodeQueue.Enqueue(this[startIndex]);
        while (nodeQueue.Count > 0)
        {
            var node = nodeQueue.Dequeue();
            visitedNodes.Add(node);

            // Enqueue unvisited child nodes
            foreach (var childNode in node.Links.Where(unvisited))
            {
                // If we encounter a gateway, return the link from the tested node to the gateway node
                if (childNode.IsGateway)
                    return new Tuple<int, int>(node.Id, childNode.Id);

                nodeQueue.Enqueue(childNode);
            }
        }

        // No path leads to a gateway
        return null;
    }

    public static SkynetNetwork Create(int numNodes, string[] links, int[] gatewayNodes)
    {
        // Create the network nodes
        var network = new SkynetNetwork
        {
            NetworkNodes = Enumerable.Range(0, numNodes)
                .Select(i => new NetworkNode
                {
                    Id = i,
                    IsGateway = gatewayNodes.Any(e => e == i)
                })
                .ToArray()
        };

        // Link up each node in the network
        foreach (var link in links.Select(l => l.Split(' ')))
        {
            network.CreateLink(int.Parse(link[0]), int.Parse(link[1]));
        }

        return network;
    }
}

public class NetworkNode
{
    public int Id { get; set; }
    public bool IsGateway { get; set; }
    public List<NetworkNode> Links { get; } = new List<NetworkNode>();
}
