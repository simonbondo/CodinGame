using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class Player
{
    static void Main(string[] args)
    {
        // 0 < width <= 30 - The number of cells on the X-axis
        int width = int.Parse(Console.ReadLine());
        // 0 < height <= 30 - The number of cells on the Y-axis
        int height = int.Parse(Console.ReadLine());

        // 'height' lines, each containing 'width' chars of either '0' for node or '.' for empty cell
        var grid = Enumerable.Range(0, height)
            .Select(i => Console.ReadLine())
            .ToArray();

        var powerNodes = PowerNode.ParseGrid(grid);

        Console.Error.WriteLine($"W:{width} H:{height} N:{powerNodes.Length}");

        foreach (var node in powerNodes)
        {
            PowerNode neighborRight = powerNodes.FirstOrDefault(n => n.X > node.X && n.Y == node.Y);
            PowerNode neighborBottom = powerNodes.FirstOrDefault(n => n.X == node.X && n.Y > node.Y);

            // For each node in grid
            // -> 3 sets of x y coords
            // --> set 1: current node
            // --> set 2: right neighbor or "-1 -1" if N/A
            // --> set 3: bottom neighbor or "-1 -1" if N/A
            Console.WriteLine($"{node.X} {node.Y} {neighborRight?.X ?? -1} {neighborRight?.Y ?? -1} {neighborBottom?.X ?? -1} {neighborBottom?.Y ?? -1}");
        }
    }
}

public class PowerNode
{
    public int X { get; set; }
    public int Y { get; set; }

    public PowerNode(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static PowerNode[] ParseGrid(string[] grid)
    {
        return grid
            .SelectMany((line, y) => line.ToCharArray()
                .Select((cell, x) => cell == '0' ? new PowerNode(x, y) : null))
            .Where(node => node != null)
            .ToArray();
    }
}
