using System;
using System.Linq;
using System.Collections;

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

        inputs = Enumerable.Range(0, height)
            .Select(n => Console.ReadLine())
            .ToArray();

        Console.Error.WriteLine($"Folds:{numFolds}, Size: {width}x{height}");

        PrintPattern(inputs);

        long solution = CalculateNumberOfAreas(inputs, numFolds, height, width);

        Console.Error.WriteLine($"Solution: {solution}");

        Console.WriteLine(solution);
    }

    public static long CalculateNumberOfAreas(string[] paper, int numFolds, int height, int width)
    {
        // Convert input to nodes
        var nodes = Node.GetNodes(paper, height, width);

        // Detect and mark unique areas
        var areaCount = 0;
        foreach (var node in nodes)
        {
            if (GetAreaSizeAndFloodfill(nodes, node, areaCount) > 0)
                areaCount++;
        }

        // Detect pattern types
        var areaPatterns = AreaPatterns.DetectPatterns(nodes, height, width);

        // Unfold patterns
        for (int i = 0; i < numFolds; i++)
        {
            areaPatterns.UnfoldPatterns();
        }

        return areaPatterns.GetTotalAreas();
    }

    public static int GetAreaSizeAndFloodfill(Node[] nodes, Node node, int areaId)
    {
        if (node == null || node.AreaId.HasValue)
            return 0;

        node.AreaId = areaId;
        return 1 +
            GetAreaSizeAndFloodfill(nodes, node.GetNodeNorth(nodes), areaId) +
            GetAreaSizeAndFloodfill(nodes, node.GetNodeWest(nodes), areaId) +
            GetAreaSizeAndFloodfill(nodes, node.GetNodeEast(nodes), areaId) +
            GetAreaSizeAndFloodfill(nodes, node.GetNodeSouth(nodes), areaId);
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

public class Node
{
    private Node nodeNorth = null;
    private Node nodeWest = null;
    private Node nodeEast = null;
    private Node nodeSouth = null;
    
    public int Row { get; set; }
    public int Column { get; set; }
    public bool HasNodeNorth { get; set; }
    public bool HasNodeWest { get; set; }
    public bool HasNodeEast { get; set; }
    public bool HasNodeSouth { get; set; }
    
    public int? AreaId { get; set; }
    
    public Node GetNodeNorth(Node[] nodes)
    {
        if (!this.HasNodeNorth)
            return null;
        
        return this.nodeNorth = nodes
            .SingleOrDefault(n => n.Row == this.Row - 1 && n.Column == this.Column);
    }
    
    public Node GetNodeWest(Node[] nodes)
    {
        if (!this.HasNodeWest)
            return null;
        
        return this.nodeWest = nodes
            .SingleOrDefault(n => n.Row == this.Row && n.Column == this.Column - 1);
    }
    
    public Node GetNodeEast(Node[] nodes)
    {
        if (!this.HasNodeEast)
            return null;
        
        return this.nodeEast = nodes
            .SingleOrDefault(n => n.Row == this.Row && n.Column == this.Column + 1);
    }
    
    public Node GetNodeSouth(Node[] nodes)
    {
        if (!this.HasNodeSouth)
            return null;
        
        return this.nodeSouth = nodes
            .SingleOrDefault(n => n.Row == this.Row + 1 && n.Column == this.Column);
    }

    public static Node[] GetNodes(string[] sheet, int height, int width)
    {
        Func<int, int, bool> isPaper = (r, c) => sheet[r][c] == '#';

        var nodes = Enumerable.Range(0, height)
            .SelectMany(r => Enumerable.Range(0, width)
                .Where(c => isPaper(r, c))
                .Select(c => new Node
                {
                    Row = r,
                    Column = c,
                    HasNodeNorth = r <= 0 ? false : isPaper(r - 1, c),
                    HasNodeWest = c <= 0 ? false : isPaper(r, c - 1),
                    HasNodeEast = c >= width - 1 ? false : isPaper(r, c + 1),
                    HasNodeSouth = r >= height - 1 ? false : isPaper(r + 1, c),
                    AreaId = null
                }))
            .ToArray();

        return nodes;
    }
}

public class AreaPatterns
{
    // Touching one edge
    public long North = 0;
    public long West = 0;
    public long East = 0;
    public long South = 0;
    
    // Touching two adjacent edges
    public long NorthWest = 0;
    public long NorthEast = 0;
    public long SouthWest = 0;
    public long SouthEast = 0;
    
    // Touching two opposite edges
    public long NorthSouth = 0;
    public long WestEast = 0;
    
    // Touching three edges
    public long NorthWestEast = 0;
    public long NorthSouthWest = 0;
    public long NorthSouthEast = 0;
    public long SouthWestEast = 0;

    // Touching four edges
    public long All = 0;
    
    // Touching no edges
    public long None = 0;
    
    public void UnfoldPatterns()
    {
        var newNorth = 2 * this.South + this.SouthWest;
        var newWest = 2 * this.East + this.NorthEast;
        var newEast = 2 * this.East + this.NorthEast;
        var newSouth = 2 * this.South + this.SouthWest;
        var newNorthWest = this.SouthEast;
        var newNorthEast = this.SouthEast;
        var newSouthWest = this.SouthEast;
        var newSouthEast = this.SouthEast;
        var newNorthSouth = 2 * this.NorthSouth + this.NorthSouthWest;
        var newWestEast = 2 * this.WestEast + this.NorthWestEast;
        var newNorthWestEast = this.SouthWestEast;
        var newNorthSouthWest = this.NorthSouthEast;
        var newNorthSouthEast = this.NorthSouthEast;
        var newSouthWestEast = this.SouthWestEast;
        var newAll = this.All;
        var newNone = 2 * this.North + 2 * this.West + this.NorthWest + 4 * this.None;
        
        this.North = newNorth;
        this.West = newWest;
        this.East = newEast;
        this.South = newSouth;
        this.NorthWest = newNorthWest;
        this.NorthEast = newNorthEast;
        this.SouthWest = newSouthWest;
        this.SouthEast = newSouthEast;
        this.NorthSouth = newNorthSouth;
        this.WestEast = newWestEast;
        this.NorthWestEast = newNorthWestEast;
        this.NorthSouthWest = newNorthSouthWest;
        this.NorthSouthEast = newNorthSouthEast;
        this.SouthWestEast = newSouthWestEast;
        this.All = newAll;
        this.None = newNone;
    }

    public long GetTotalAreas()
    {
        return this.North +
            this.West +
            this.East +
            this.South +
            this.NorthWest +
            this.NorthEast +
            this.SouthWest +
            this.SouthEast +
            this.NorthSouth +
            this.WestEast +
            this.NorthWestEast +
            this.NorthSouthWest +
            this.NorthSouthEast +
            this.SouthWestEast +
            this.All +
            this.None;
    }

    public static AreaPatterns DetectPatterns(Node[] nodes, int height, int width)
    {
        // Group nodes by area
        var areaNodes = nodes
            .GroupBy(n => n.AreaId)
            .ToArray();

        var patterns = new AreaPatterns();
        foreach (var area in areaNodes)
        {
            // Detect which edges are touched by current area
            var touchingNorth = area.Any(n => n.Row == 0);
            var touchingWest = area.Any(n => n.Column == 0);
            var touchingEast = area.Any(n => n.Column == width - 1);
            var touchingSouth = area.Any(n => n.Row == height - 1);

            // Four edges
            if (!touchingNorth && !touchingWest && !touchingEast && !touchingSouth)
                patterns.None++;
            else if (touchingNorth && touchingWest && touchingEast && touchingSouth)
                patterns.All++;
            // Three edges
            else if (touchingNorth && touchingWest && touchingEast)
                patterns.NorthWestEast++;
            else if (touchingNorth && touchingSouth && touchingWest)
                patterns.NorthSouthWest++;
            else if (touchingNorth && touchingSouth && touchingEast)
                patterns.NorthSouthEast++;
            else if (touchingSouth && touchingWest && touchingEast)
                patterns.SouthWestEast++;
            // Two opposite edges
            else if (touchingNorth && touchingSouth)
                patterns.NorthSouth++;
            else if (touchingWest && touchingEast)
                patterns.WestEast++;
            // Two adjacent edges
            else if (touchingNorth && touchingWest)
                patterns.NorthWest++;
            else if (touchingNorth && touchingEast)
                patterns.NorthEast++;
            else if (touchingSouth && touchingWest)
                patterns.SouthWest++;
            else if (touchingSouth && touchingEast)
                patterns.SouthEast++;
            // One edge
            else if (touchingNorth)
                patterns.North++;
            else if (touchingWest)
                patterns.West++;
            else if (touchingEast)
                patterns.East++;
            else if (touchingSouth)
                patterns.South++;
            else
            {
                throw new NotImplementedException($"Pattern not implemented. Area:{area.Key} n:{touchingNorth} w:{touchingWest} e:{touchingEast} s:{touchingSouth}");
            }
        }
        
        return patterns;
    }
}
