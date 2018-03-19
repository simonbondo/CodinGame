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
class Solution
{
    static void Main(string[] args)
    {
        var LON = double.Parse(Console.ReadLine().Replace(',', '.'));
        var LAT = double.Parse(Console.ReadLine().Replace(',', '.'));
        var N = int.Parse(Console.ReadLine());

        // Parse inputs into Defib objects
        var defibs = Enumerable.Range(0, N)
            .Select(n => Console.ReadLine())
            .Select(Defib.Parse)
            .ToArray();

        Console.Error.WriteLine($"My location, lo:{LON} la:{LAT}");

        // Get the closest defib
        var closest = defibs
            .OrderBy(defib => defib.GetDistance(LON, LAT))
            .First();

        Console.Error.WriteLine($"Closest defib is number {closest.Number}");
        Console.WriteLine(closest.Name);
    }

    class Defib
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Defib(int number, string name, string address, string phone, double longitude, double latitude)
        {
            this.Number = number;
            this.Name = name;
            this.Address = address;
            this.Phone = phone;
            this.Longitude = longitude;
            this.Latitude = latitude;

            Console.Error.WriteLine($"{this.Number}:{this.Name} == lo:{this.Longitude} la:{this.Latitude}");
        }

        public double GetDistance(double fromLong, double fromLat)
        {
            // Copied formula from the puzzle introduction 😕
            var x = (this.Longitude - fromLong) * Math.Cos((fromLat + this.Latitude) / 2);
            var y = this.Latitude - fromLat;
            // 6371 is supposedly the radius of the earth in km
            var d = Math.Sqrt(x * x + y * y) * 6371d;

            Console.Error.WriteLine($"Distance to number {this.Number} is {d} in radians");
            return d;
        }

        public static Defib Parse(string input)
        {
            var parts = input.Split(';');
            return new Defib(int.Parse(parts[0]), parts[1], parts[2], parts[3], double.Parse(parts[4].Replace(',', '.')), double.Parse(parts[5].Replace(',', '.')));
        }
    }
}
