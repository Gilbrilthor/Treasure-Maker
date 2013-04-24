using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TreasureGenerator
{
    public class TreasureGenerator
    {
        private static Random rand = null;
        private List<TreasureEntry> entries;

        public TreasureGenerator(string filename)
        {
            // If there isn't a random class, make a new one
            if (rand == null) rand = new Random();

            entries = parseFile(filename);
        }

        public TreasureResult GenerateTreasure(int maxValue = -1)
        {
            // Get the lower and upper bounds
            int minBound = Int32.MaxValue,
                maxBound = Int32.MinValue;
            foreach (TreasureEntry e in entries)
            {
                if (e.LowerBound < minBound)
                    minBound = e.LowerBound;

                if (e.HigherBound > maxBound)
                    maxBound = e.HigherBound;
            }

            TreasureResult result = null;

            do
            {
                // Get a random value and see which entry it matches
                int randInt = rand.Next(minBound, maxBound);

                TreasureEntry entry = null;
                for (int i = 0; i < entries.Count && entry == null; i++)
                {
                    if (randInt >= entries[i].LowerBound && randInt <= entries[i].HigherBound)
                    {
                        entry = entries[i];
                    }
                }

                randInt = rand.Next(entry.MinWorth, entry.MaxWorth);
                string desc = entry.Examples[rand.Next(entry.Examples.Count)]; 

                result = new TreasureResult(randInt, desc);
            }
            while ( maxValue != -1 && result.Worth > maxValue);

            return result;
        }

        private List<TreasureEntry> parseFile(string filename)
        {
            List<TreasureEntry> results = new List<TreasureEntry>();

            StreamReader file = new StreamReader(filename);

            string line;
            while(file.Peek() != -1)
            {
                line = file.ReadLine();

                try
                {
                    results.Add(new TreasureEntry(line));
                }
                catch (Exception)
                {
                    // Don't worry about errors, just skip the line
                }
            }

            file.Close();

            return results;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TreasureEntry g in entries)
            {
                sb.AppendLine(g.ToString());
            }

            return sb.ToString();
        }
    }

    public class TreasureResult
    {
        private int worth;
        public int Worth
        {
            get { return worth; }
            // Make sure worth is not negative
            private set
            {
                if (value >= 0)
                    worth = value;
                else
                    throw new Exception("Worth cannot be negative!");
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            private set { description = value; }
        }

        public TreasureResult(int worth, string description)
        {
            Worth = worth;
            Description = description;
        }

        public override string ToString()
        {
            return String.Format("A {0} worth {1}", description, worth);
        }
    }
    public class TreasureEntry
    {
        private int lowBound;
        public int LowerBound
        {
            get { return lowBound; }
            set { lowBound = value; }
        }

        private int highBound;
        public int HigherBound
        {
            get { return highBound; }
            set { highBound = value; }
        }

        private int lowCost;
        public int MinWorth
        {
            get { return lowCost; }
            set { lowCost = value; }
        }

        private int highCost;
        public int MaxWorth
        {
            get { return highCost; }
            set { highCost = value; }
        }

        private int averageCost;
        public int AverageWorth
        {
            get { return averageCost; }
            set { averageCost = value; }
        }

        private List<string> examples;
        public List<string> Examples
        {
            get { return examples; }
            private set { examples = value; }
        }

        public TreasureEntry()
        {
            lowBound = 0;
            highBound = 100;

            lowCost = 0;
            highCost = 100;
            averageCost = 50;

            examples = new List<string>();

            examples.Add("Test");
        }

        public TreasureEntry(string EntryString)
        {
            // Create a new list
            examples = new List<string>();
            
            // parse the entry in
            parseDatFileString(EntryString);
        }

        private void parseDatFileString(string EntryString)
        {
            string[] fields = EntryString.Split('|');

            if (fields.Length < 4)
                throw new Exception(String.Format("Malformed entry: '{0}'", EntryString));
            else
            {
                // Parse the bounds
                string[] intsStrings = fields[0].Split('-');
                lowBound = Int32.Parse(intsStrings[0]);
                highBound = Int32.Parse(intsStrings[1]);

                // Parse the costs
                intsStrings = fields[1].Split('-');
                lowCost = Int32.Parse(intsStrings[0]);
                highCost = Int32.Parse(intsStrings[1]);

                // Parse the average cost
                averageCost = Int32.Parse(fields[2]);

                // Parse the examples into their own strings. delimited by ;
                examples = parseExamples(fields[3]);
            }
        }

        private List<string> parseExamples(string exampleLine)
        {
            // Break the line up by the semicolons
            string[] results = exampleLine.Split(';');

            // trim the strings and add them to the list
            List<string> list = new List<string>();

            foreach(string s in results)
            {
                if (s != String.Empty)
                    list.Add(s.Trim());
            }

            // return the list
            return list;
        }

        public override string ToString()
        {
            return String.Format("Bounds: {0} - {1}\n" +
                    "Worth: {2} - {3} (Avg: {4})\n" +
                    "Examples: {5}",
                    lowBound, highBound,
                    lowCost, highCost, averageCost,
                    String.Join(" ", examples.ToArray()));
        }

    }
}
