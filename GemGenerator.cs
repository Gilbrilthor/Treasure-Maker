using System;
using System.Collections.Generic;

namespace GemGenerator
{
    public class GemGenerator
    {

    }

    public class GemEntry
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

        public GemEntry()
        {
            lowBound = 0;
            highBound = 100;

            lowCost = 0;
            highCost = 100;
            averageCost = 50;

            examples = new List<string>();

            examples.Add("Test");
        }

        public GemEntry(string EntryString)
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
                string[] intsStrings = fields[0].Split('|');
                lowBound = Int32.Parse(intsStrings[0]);
                highBound = Int32.Parse(intsStrings[1]);

                // Parse the costs
                intsStrings = fields[1].Split('|');
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
                list.Add(s.Trim());
            }

            // return the list
            return list;
        }

    }
}
