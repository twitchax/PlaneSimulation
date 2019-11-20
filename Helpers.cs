using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaneSimulation
{
    internal static class Helpers
    {
        //internal static ThreadSafeRandom Random = new ThreadSafeRandom();

        internal static bool RunSimulationManaged(int numSeats)
        {
            var rand = new Random();

            // Create list of people with ticket numbers 1 - {numSeats}.
            var peopleTickets = Enumerable.Range(1, numSeats).ToList();

            // Assign seat numbers "randomly".
            peopleTickets.Shuffle(rand);

            // Create a dictionary (O(1) lookup of seats to see if someone has taken the seat).
            // `true` is taken, `false` is empty.
            var seats = Enumerable.Range(1, numSeats).ToDictionary(n => n, n => false);

            // The first person drops their ticket and chooses a seat at Random.
            peopleTickets[0] = rand.Next(numSeats) + 1;

            // Have everyone but you take a seat.
            foreach(var ticket in peopleTickets.Take(numSeats - 1))
            {
                if(seats[ticket]) // `true` means the seat is taken.
                {
                    // Get open seats.
                    var openSeats = seats.Where(kvp => !kvp.Value).Select(kvp => kvp.Key).ToList();

                    // Choose a random seat.
                    var randomSeat = openSeats[rand.Next(openSeats.Count)];

                    // Sit in that random seat.
                    seats[randomSeat] = true;
                }
                else // `false` means the seat is open.
                {
                    // Sit in the seat.
                    seats[ticket] = true;
                }
            }

            // Check to see if your seat is open, and return `true` if you got your own seat.
            var yourSeat = peopleTickets.Last();

            return !seats[yourSeat];
        }

        internal static bool RunSimulationArrays(int numSeats)
        {
            var rand = new Random();

            // Create list of people with ticket numbers 1 - {numSeats}.
            Span<int> peopleTickets = stackalloc int[numSeats];
            for(int k = 0; k < numSeats; k++)
                peopleTickets[k] = k;

            // Assign seat numbers "randomly".
            peopleTickets.Shuffle(rand);

            // `true` is taken, `false` is empty.
            Span<bool> seats = stackalloc bool[numSeats];
            for(int k = 0; k < numSeats; k++)
                seats[k] = false;

            // The first person drops their ticket and chooses a seat at Random.
            peopleTickets[0] = rand.Next(numSeats);

            // Have everyone but you take a seat.
            for(int k = 0; k < numSeats - 1; k++)
            {
                var ticket = peopleTickets[k];

                if(seats[ticket]) // `true` means the seat is taken.
                {
                    var numOpenSeats = numSeats - k;

                    // Get open seats.
                    Span<int> openSeats = stackalloc int[numOpenSeats];
                    var o = 0;
                    for(int j = 0; j < numSeats; j++)
                        if(!seats[j])
                        {
                            openSeats[o] = j;
                            o++;
                        }

                    // Choose a random seat.
                    var randomSeat = openSeats[rand.Next(numOpenSeats)];

                    // Sit in that random seat.
                    seats[randomSeat] = true;
                }
                else // `false` means the seat is open.
                {
                    // Sit in the seat.
                    seats[ticket] = true;
                }
            }

            // Check to see if your seat is open, and return `true` if you got your own seat.
            var yourSeat = peopleTickets[numSeats - 1];

            return !seats[yourSeat];
        }

        internal static void Shuffle<T>(this IList<T> list, Random rand)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rand.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        internal static void Shuffle<T>(this Span<T> list, Random rand)
        {
            var n = list.Length;
            while (n > 1)
            {
                n--;
                var k = rand.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}