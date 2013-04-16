using System;
using GemGenerator;

namespace GemGenerator
{
    public class Driver
    {
        static public void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GemEntry entry = new GemEntry();

            Console.WriteLine(entry.AverageWorth);

            return;
        }
    }
}
