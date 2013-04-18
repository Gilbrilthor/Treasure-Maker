using System;
using GemGenerator;
using System.IO;

namespace GemGenerator
{
    public class Driver
    {
        static public void Main(string[] args)
        {
            string filename = "data/gems.dat";
            Console.WriteLine("Reading from" + filename);

            // Test the parsing of a file
            GemGenerator gen = new GemGenerator(filename);

            Console.WriteLine(gen);

            Console.WriteLine("Testing for empty Strings...");
            for (int i = 0; i < 1000; i++)
            {
                GemResult result = gen.GenerateGem();
                if (result.Description == String.Empty)
                    Console.WriteLine(result);
            }
            Console.WriteLine("Finished!");
            Console.WriteLine("Testing for max value limits...");
            for (int i = 1; i < 1000; i++)
            {
                GemResult result = gen.GenerateGem(i * 10);
                if (result.Worth > i * 10)
                    Console.WriteLine("Error! Results worth ({0}) is greater than {1}!", result.Worth, i * 10);
            }
            Console.WriteLine("Finished!");

            return;
        }
    }
}
