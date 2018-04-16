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
        Func<string, int> getCardValue = c => {
            switch (c[0])
            {
                case 'J': return 11;
                case 'Q': return 12;
                case 'K': return 13;
                case 'A': return 14;
                default: return c[0] - 48;
            }
        };
        var p1Deck = new Queue<int>(Enumerable.Range(0, int.Parse(Console.ReadLine())).Select(l => Console.ReadLine()).Select(getCardValue));
        var p2Deck = new Queue<int>(Enumerable.Range(0, int.Parse(Console.ReadLine())).Select(l => Console.ReadLine()).Select(getCardValue));

        Func<Queue<int>, Queue<int>, Tuple<int, int>> startFight = (p1, p2) => new Tuple<int, int>(p1.Dequeue(), p2.Dequeue());
        Action<Tuple<int, int>> p1WinsFight = fight => { p1Deck.Enqueue(fight.Item1); p1Deck.Enqueue(fight.Item2); };
        Action<Tuple<int, int>> p2WinsFight = fight => { p2Deck.Enqueue(fight.Item1); p2Deck.Enqueue(fight.Item2); };

        var roundsPlayed = 0;
        while (p1Deck.Count > 0 && p2Deck.Count > 0)
        {
            roundsPlayed++;
            Console.Error.WriteLine($"Round:{roundsPlayed}, DeckSize P1:{p1Deck.Count}, P2:{p2Deck.Count}");

            var fight = startFight(p1Deck, p2Deck);

            if (fight.Item1 > fight.Item2)
                p1WinsFight(fight);
            else if (fight.Item1 < fight.Item2)
                p2WinsFight(fight);
            else
            {
                Console.Error.WriteLine($"WAR not implemented: {fight.Item1} vs. {fight.Item2}");
                /*
                var p1Army = new[]{p1Deck.Dequeue(),p1Deck.Dequeue(),p1Deck.Dequeue()};
                var p2Army = new[]{p2Deck.Dequeue(),p2Deck.Dequeue(),p2Deck.Dequeue()};

                var newFight = startFight(p1Deck, p2Deck);

                if (newFight.Item1 > newFight.Item2)
                {
                    p1Deck.Enqueue(fight.Item1);
                    foreach (var item in p1Army)
                        p1Deck.Enqueue(item);
                    p1Deck.Enqueue(newFight.Item1);
                    p1Deck.Enqueue(fight.Item2);
                    foreach (var item in p2Army)
                        p1Deck.Enqueue(item);
                    p1Deck.Enqueue(newFight.Item2);
                }
                else if (newFight.Item1 < newFight.Item2)
                {
                    p2Deck.Enqueue(fight.Item1);
                    foreach (var item in p1Army)
                        p2Deck.Enqueue(item);
                    p2Deck.Enqueue(newFight.Item1);
                    p2Deck.Enqueue(fight.Item2);
                    foreach (var item in p2Army)
                        p2Deck.Enqueue(item);
                    p2Deck.Enqueue(newFight.Item2);
                }
                else
                    Console.Error.WriteLine($"Recursive WAR not implemented: {newFight.Item1} vs. {newFight.Item2}");*/
            }
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        if (p1Deck.Count > 0)
            Console.WriteLine($"1 {roundsPlayed}");
        else if (p2Deck.Count > 0)
            Console.WriteLine($"2 {roundsPlayed}");
        
        // TODO: Tie not implemented
        //Console.WriteLine("PAT");
    }
}
