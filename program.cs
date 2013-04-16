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

            StreamReader file = new StreamReader(filename);

            // Test the GemEntry with a line from the data file
            GemEntry entry = new GemEntry(file.ReadLine());

            Console.WriteLine(entry);

            file.Close();

            return;
        }
    }
}
