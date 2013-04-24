using System;
using System.IO;

namespace ConsoleHelper
{
    public class Helper
    {
        static public int GetIntFromUser(string prompt = "Please enter an integer: ", bool retry = true)
        {
            string input;
            bool correct = false;
            int result = 0;

            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                correct = int.TryParse(input, out result);
                if (!correct)
                    Console.WriteLine("'{0}' is not a valid integer!", input);
            }
            while(retry && !correct);

            return result;
        }
    }
}
