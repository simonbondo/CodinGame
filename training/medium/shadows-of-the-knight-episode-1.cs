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
class Player
{
    static void Main(string[] args)
    {
        var inputs = Console.ReadLine().Split(' ');
        // width of the building in terms of number of windows
        var width = int.Parse(inputs[0]);
        // height of the building in terms of number of windows
        var height = int.Parse(inputs[1]);
        // maximum number of turns before game over.
        var allowedTurns = int.Parse(Console.ReadLine());

        inputs = Console.ReadLine().Split(' ');
        // Initial position of batman
        var x = int.Parse(inputs[0]);
        var y = int.Parse(inputs[1]);

        // Maintain a virtual bounding box wherein the bomb is known to be
        var top = 0;
        var left = 0;
        var right = width - 1;
        var bottom = height - 1;
    
        // game loop
        while (true)
        {
            var bombDirection = GetBombDirection();

            // Calculate the point halfway between current position and the bounding box in the direction of the bomb
            // Update virtual border
            if ((bombDirection & Directions.Up) == Directions.Up)
            {
                bottom = y;
                y -= (int)Math.Ceiling((y - top) / 2d);
            }
            else if ((bombDirection & Directions.Down) == Directions.Down)
            {
                top = y;
                y += (int)Math.Ceiling((bottom - y) / 2d);
            }

            if ((bombDirection & Directions.Left) == Directions.Left)
            {
                right = x;
                x -= (int)Math.Ceiling((x - left) / 2d);
            }
            else if ((bombDirection & Directions.Right) == Directions.Right)
            {
                left = x;
                x += (int)Math.Ceiling((right - x) / 2d);
            }

            Console.Error.WriteLine($"Box T:{top} R:{right} B:{bottom} L:{left}");

            // the location of the next window Batman should jump to.
            Console.WriteLine($"{x} {y}");
        }
    }

    public static Directions GetBombDirection()
    {
        var input = Console.ReadLine();

        var direction = Directions.None;

        if (input.Contains('U'))
            direction |= Directions.Up;
        else if (input.Contains('D'))
            direction |= Directions.Down;

        if (input.Contains('R'))
            direction |= Directions.Right;
        else if (input.Contains('L'))
            direction |= Directions.Left;

        return direction;
    }

    [Flags]
    public enum Directions
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
    }
}
