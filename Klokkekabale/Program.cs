using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klokkekabale
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var r = new Random(Guid.NewGuid().GetHashCode());
            var successes = 0;
            var attempts = 100000;
            for (int i = 0; i < attempts; ++i)
            {
                if (OneTurn(r))
                {
                    successes++;
                }
            }

            Console.WriteLine($"{100F * successes / attempts} % af kabalerne går op");

            Console.ReadKey();
        }

        static bool OneTurn(Random r)
        { 
            // Set up clock
            var clock = new List<int>[14];
            for (int i = 1; i <= 13; i++)
            {
                clock[i] = new List<int>();
            }

            // Create sorted deck
            var sortedDeck = new List<int>();
            for (int b = 1; b <= 4; ++b)
            {
                for (int a = 1; a <= 13; ++a)
                {
                    sortedDeck.Add(a);
                }
            }

            // Create scrambled deck
            var deck = new Queue<int>();
            var max = sortedDeck.Count;
            for (int a = 0; a < max; ++a)
            {
                var pos = r.Next(sortedDeck.Count());
                var card = sortedDeck[pos];
                deck.Enqueue(card);
                sortedDeck[pos] = sortedDeck[sortedDeck.Count - 1];
                sortedDeck.RemoveAt(sortedDeck.Count - 1);
            }

            // Start game
            var time = 1;
            var left = 13;
            while (deck.Count > 0)
            {
                // Pick a card
                var card = deck.Dequeue();

                // Add card to clock
                clock[time].Add(card);

                // If card is correct, move bottom of clock to deck
                if (card == time)
                {
                    var tail = clock[time].Take(clock[time].Count() - 1);
                    foreach (var tailCard in tail.Reverse())
                    {
                        deck.Enqueue(tailCard);
                    }
                    
                    clock[time] = null;

                    if (--left == 0)
                    {
                        break;
                    }
                }

                do
                {
                    // Increase time
                    time++;

                    // If 14, go back to 1
                    if (time == 14)
                    {
                        time = 1;
                    }
                }
                while(clock[time] == null);
            }

            return left == 0;
        }
    }
}
