using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


static class Consts
{
    public const int MaxThrust = 100;
    
    // Used for scaling the soft curve of the braking algorithm.
    // Lower value compresses the curve, where 1d will make the braking almost binary
    public const double BrakingFactorDistanceScaling = 1000d;
    // When the scaling is changed, the distance should also be changed
    public const int StartBrakingWhenDistanceLessThan = 3000;
    
    // Prevent stopping by never applying more than this amount of braking from short distances
    public const double MaxBrakingFactorByDistance = .95d;
    
    // Used for scaling the soft curve of the braking algorithm.
    // Lower value compresses the curve, where 1d will make the braking almost binary
    public const double BrakingFactorAngleScaling = 20d;
    public const int StartBrakingWhenAngleGreaterThan = 45;
    
    // Prevent stopping by never applying more than this amount of braking from steep angles
    public const double MaxBrakingFactorByAngle = .7d;
    
    // Adjust when the BOOST should be used
    public const int MinDistanceToUseBoost = 5000;
    public const int MaxAngleToUseBoost = 5;
    public const int NumIterationsBoostDistanceSatisfied = 6;
}

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;

        var boostUsed = false;
        var boostSatisfiedIterationCount = 0;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int x = int.Parse(inputs[0]);
            int y = int.Parse(inputs[1]);
            int nextCheckpointX = int.Parse(inputs[2]); // x position of the next check point
            int nextCheckpointY = int.Parse(inputs[3]); // y position of the next check point
            int nextCheckpointDist = int.Parse(inputs[4]); // distance to the next checkpoint
            int nextCheckpointAngle = int.Parse(inputs[5]); // angle between your pod orientation and the direction of the next checkpoint
            inputs = Console.ReadLine().Split(' ');
            int opponentX = int.Parse(inputs[0]);
            int opponentY = int.Parse(inputs[1]);

            // =====

            var thrustByDistance = GetThrustByDistance(nextCheckpointDist);
            var thrustByAngle = GetThrustByAngle(nextCheckpointAngle);
            
            // Average the thrust from distance and angle calculation
            var thrustAverage = new[] { thrustByDistance, thrustByAngle }.Average();
            var thrustAmount = (int)Math.Round(thrustAverage);

            var boostReady = false;
            if (!boostUsed)
            {
                if (IsBoostSatisfied(nextCheckpointDist, nextCheckpointAngle))
                    boostSatisfiedIterationCount++;
                else
                    boostSatisfiedIterationCount = 0;

                if (boostSatisfiedIterationCount >= Consts.NumIterationsBoostDistanceSatisfied)
                    boostReady = boostUsed = true;
            }
            
            Console.Error.WriteLine($"Thrust 'distance:{thrustByDistance}', 'angle:{thrustByAngle}', 'average:{thrustAverage}'");
            Console.Error.WriteLine($"Boost 'used:{boostUsed}', 'ready:{boostReady}', 'iterationCount:{boostSatisfiedIterationCount}'");

            var thrust = boostReady
                ? "BOOST"
                : thrustAmount.ToString();

            // =====
            
            Console.WriteLine($"{nextCheckpointX} {nextCheckpointY} {thrust} {thrustAverage}");
        }
    }
    
    private static int GetThrustByDistance(int distance)
    {
        // Calculate how much to reduce the thrust
        var brakingFactor = GetBrakingFactorByDistance(distance);
        var thrustFactor = 1d - brakingFactor;
        
        var thrust = Consts.MaxThrust * thrustFactor;
        return (int)Math.Round(thrust);
    }
    
    private static double GetBrakingFactorByDistance(int distance)
    {
        // Don't apply any braking when distance is greater than the threshold
        if (distance >= Consts.StartBrakingWhenDistanceLessThan)
            return 0d;
        
        // Scale the distance to make the TanH method to work better
        var scaledDistance = distance / Consts.BrakingFactorDistanceScaling;
        var brakingFactor = 1d - Math.Tanh(scaledDistance);
        
        // Clamp the braking factor
        return Math.Min(brakingFactor, Consts.MaxBrakingFactorByDistance);
    }
    
    private static int GetThrustByAngle(int angle)
    {
        // Use the absolute angle to avoid mirroring the calculations
        angle = Math.Abs(angle);
        
        // Calculate how much to reduce the thrust
        var brakingFactor = GetBrakingFactorByAngle(angle);
        var thrustFactor = 1d - brakingFactor;
        
        var thrust = Consts.MaxThrust * thrustFactor;
        return (int)Math.Round(thrust);
    }
    
    private static double GetBrakingFactorByAngle(int angle)
    {
        // Don't apply any braking when angle is smaller than the threshold
        if (angle <= Consts.StartBrakingWhenAngleGreaterThan)
            return 0d;
        
        // Since the desired angle is low, the value needs to be shifted and then scaled for the TanH method to work better
        var shiftedAngle = angle - Consts.StartBrakingWhenAngleGreaterThan;
        var scaledAngle = Consts.BrakingFactorAngleScaling / shiftedAngle;
        var brakingFactor = 1d - Math.Tanh(scaledAngle);
        
        // Clamp the braking factor
        return Math.Min(brakingFactor, Consts.MaxBrakingFactorByAngle);
    }
    
    private static bool IsBoostSatisfied(int distance, int angle)
    {
        return distance >= Consts.MinDistanceToUseBoost
            && angle <= Consts.MaxAngleToUseBoost;
    }
}
