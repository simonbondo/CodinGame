using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * ---
 * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
 **/
class Player
{
    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int lightX = int.Parse(inputs[0]); // the X position of the light of power
        int lightY = int.Parse(inputs[1]); // the Y position of the light of power
        int initialTX = int.Parse(inputs[2]); // Thor's starting X position
        int initialTY = int.Parse(inputs[3]); // Thor's starting Y position

        var currentTX = initialTX;
        var currentTY = initialTY;

        // game loop
        while (true)
        {
            int remainingTurns = int.Parse(Console.ReadLine()); // The remaining amount of turns Thor can move. Do not remove this line.

            var speedX = lightX.CompareTo(currentTX);
            var speedY = lightY.CompareTo(currentTY);

            var direction = string.Empty;

            if (speedY > 0)
                direction += "S";
            else if (speedY < 0)
                direction += "N";
            
            if (speedX > 0)
                direction += "E";
            else if (speedX < 0)
                direction += "W";

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.Error.WriteLine($"From '{initialTX}:{initialTY}' to {lightX}:{lightY} with remaining energy {remainingTurns}");
            Console.Error.WriteLine($"Currently at '{currentTX}:{currentTY}' going at speed '{speedX}:{speedY}'");

            // A single line providing the move to be made: N NE E SE S SW W or NW
            Console.WriteLine(direction);

            // Update current location of Thor based on speed
            currentTX += speedX;
            currentTY += speedY;
        }
    }
}
