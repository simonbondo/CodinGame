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
        var area = AreaData.ReadInputs();

        // game loop
        while (true)
        {
            var turn = TurnData.ReadInputs();

            if (turn.LeadingCloneDirection == CloneDirections.None)
            {
                Console.WriteLine("WAIT");
                continue;
            }

            var goalPos = turn.LeadingCloneFloor == area.ExitFloor
                ? area.ExitPosition
                : area.Elevators.FirstOrDefault(e => e.Floor == turn.LeadingCloneFloor)?.Position;
            
            if (goalPos == turn.LeadingClonePosition)
            {
                Console.WriteLine("WAIT");
                continue;
            }

            var targetDirection = goalPos > turn.LeadingClonePosition
                ? CloneDirections.Right
                : CloneDirections.Left;
            
            if (turn.LeadingCloneDirection == targetDirection)
                Console.WriteLine("WAIT");
            else
                Console.WriteLine("BLOCK");
        }
    }

    public class TurnData
    {
        // floor of the leading clone
        public int LeadingCloneFloor { get; private set; }
        // position of the leading clone on its floor
        public int LeadingClonePosition { get; private set; }
        // direction of the leading clone: LEFT or RIGHT - NONE if there are no active clones in the area
        public CloneDirections LeadingCloneDirection { get; private set; }

        public static TurnData ReadInputs()
        {
            var inputs = Console.ReadLine().Split(' ');

            return new TurnData
            {
                LeadingCloneFloor = int.Parse(inputs[0]),
                LeadingClonePosition = int.Parse(inputs[1]),
                LeadingCloneDirection = (CloneDirections)Enum.Parse(typeof(CloneDirections), inputs[2], true)
            };
        }
    }

    public class AreaData
    {
        // 1 <= NumberOfFloors <= 15 - floors in the area
        public int NumberOfFloors { get; private set; }
        // 5 <= AreaWidth <= 100 - width of the area
        public int AreaWidth { get; private set; }
        // 10 <= MaxRounds <= 200 - maximum number of rounds
        public int MaxRounds { get; private set; }
        // 0 <= ExitFloor < numFloors - floor on which the exit is found
        public int ExitFloor { get; private set; }
        // 0 <= ExitPosition < width - position of the exit on its floor
        public int ExitPosition { get; private set; }
        // 2 <= NumberOfTotalClones <= 50 - number of generated clones
        public int NumberOfTotalClones { get; private set; }
        // ignore (always zero)
        public int NumberOfAdditionalElevators { get; private set; }
        // 0 <= NumberOfElevators <= 100 - number of elevators
        public int NumberOfElevators { get; private set; }
        public Elevator[] Elevators { get; private set; }

        public static AreaData ReadInputs()
        {
            var inputs = Console.ReadLine().Split(' ');

            var input = new AreaData
            {
                NumberOfFloors = int.Parse(inputs[0]),
                AreaWidth = int.Parse(inputs[1]),
                MaxRounds = int.Parse(inputs[2]),
                ExitFloor = int.Parse(inputs[3]),
                ExitPosition = int.Parse(inputs[4]),
                NumberOfTotalClones = int.Parse(inputs[5]),
                NumberOfAdditionalElevators = int.Parse(inputs[6]),
                NumberOfElevators = int.Parse(inputs[7])
            };

            input.Elevators = Enumerable.Range(0, input.NumberOfElevators)
                .Select(i => Console.ReadLine().Split(' '))
                .Select(i => new Elevator
                {
                    Floor = int.Parse(i[0]),
                    Position = int.Parse(i[1])
                })
                .ToArray();

            return input;
        }
    }

    public enum CloneDirections
    {
        None,
        Right,
        Left
    }

    public class Elevator
    {
        // 0 <= Floor < numFloors
        public int Floor { get; set; }
        // 0 <= Position < width
        public int Position { get; set; }
    }
}
