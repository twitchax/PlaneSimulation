using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlaneSimulation
{
    class Program
    {
        static readonly int NumSimulations = 1_000_000;
        static readonly int NumSeats = 100;

        static void Main(string[] args)
        {
            var foundCount = 0;
            var average = 0d;
            
            var start = DateTime.Now;
            var result = Parallel.For(1, NumSimulations + 1, (k, state) =>
            {
                var foundSeat = Helpers.RunSimulationArrays(NumSeats);
                if(foundSeat)
                    Interlocked.Increment(ref foundCount);
            });
            var end = DateTime.Now;

            average = 1.0 * foundCount / NumSimulations;

            Console.WriteLine($"You get your seat approximately {average*100,0:00.00}% of the time.");
            Console.WriteLine($"Completed in {(end - start).TotalSeconds,0:00.00} seconds.");
        }
    }
}
