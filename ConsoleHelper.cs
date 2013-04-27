using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleHelper
{
    public class Helper
    {
        static public void WaitForKey(string predicate = "continue")
        {
            Console.Write("Press any key to {0}.", predicate);
            Console.ReadKey();
        }

        static public int GetIntFromUser(string prompt = "Please enter an integer: ",
                Predicate<int> validator = null, bool retry = true)
        {
            string input;
            bool correct = false;
            int result = 0;

            do
            {
                // Display the prompt
                Console.Write(prompt);
                // Get the line of input and try to parse it to an integer
                input = Console.ReadLine();
                correct = int.TryParse(input, out result);

                // If the validator is present, use it to validate the integer
                if (correct)
                    correct = (validator != null? validator(result) : true);

                // If the result is not valid, display an error
                if (!correct)
                    Console.WriteLine("'{0}' is not a valid integer!", input);
            }
            while(retry && !correct);

            return result;
        }

        static public string GetStringFromUser(
                string prompt = "Please enter a string: ",
                Predicate<string> validator = null,
                string errorMessage = "'{0}' is not a valid string!",
                bool retry = true)
        {
            string input;
            bool correct = false;

            do
            {
                // Output a prompt
                Console.Write(prompt);
                // Get the input from the user
                input = Console.ReadLine();
                // If there was a validator, use it to determine correctness
                correct = (validator != null? validator(input): true);
                // Display error message if it wasn't correct
                if (!correct)
                {
                    if(errorMessage.Contains("{0}") && 
                            System.Text.RegularExpressions.Regex.IsMatch(
                                errorMessage, @"\{[1-9]*\}") == false)
                        Console.WriteLine(errorMessage, input);
                    else
                        Console.WriteLine(errorMessage);
                }
            }
            while(retry && !correct);   // Keep trying if we need to

            // Return the final string
            return input;
        }
    }

    public class Menu
    {
        private SortedList<int, MenuItem> menuItems;

        private MenuItem QuitOption
        {
            get { return new MenuItem(menuItems.Count, QuitText, null); }
        }

        public string Header { get; set; }
        public string QuitText { get; set; }

        public Menu()
        {
            menuItems = new SortedList<int, MenuItem>();
            menuItems.Add(0, new MenuItem(0, "Quit", null));
            Header = "What would you like to do?";
            QuitText = "Quit";
        }

        public Menu(IEnumerable<MenuItem> menuItems, string header = null, string quitText = null)
        {
            this.menuItems = new SortedList<int, MenuItem>();

            foreach(MenuItem m in menuItems)
                this.menuItems.Add(m.Number, m);

            if (header == null)
                Header = "What would you like to do?";
            else
                Header = header;

            if (quitText == null)
                QuitText = "Quit";
            else
                QuitText = quitText;
        }
        

        public static void Debug()
        {
            MenuItem.Debug();
            Console.WriteLine("Testing getMenuChoice!");

            MenuItem[] items = new MenuItem[3];
            items[0] = new MenuItem(0, "one", delegate() { Console.WriteLine("ONE!");}); 
            items[1] = new MenuItem(1, "two");
            items[2] = new MenuItem(2, "three");

            Menu menu = new Menu(items);

            menu.RunMenu();

            Console.WriteLine("Finished Testing getMenuChoice");
        }

        public void RunMenu(bool repeat = true)
        {
            MenuItem choice = null;

            do
            {
                DisplayMenu();
                choice = getMenuChoice();
                choice.Execute();
                if (choice != QuitOption)
                    Helper.WaitForKey();

            } while (repeat && choice != QuitOption);

            Console.Clear();
        }


        public void DisplayMenu()
        {
            // Clear the console
            Console.Clear();

            // Write the header
            Console.WriteLine(Header);
            // Display the items in the menu
            foreach (MenuItem item in menuItems.Values)
                item.DisplayMenuItem();

            QuitOption.DisplayMenuItem();
        }

        public MenuItem getMenuChoice(string prompt = "Please enter a number: ",
                string errMessage = "You must enter a valid number! ")
        {
            string input;
            int choice;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();

                // Try to parse it. If it doesn't work, print the error and keep going
                if (!Int32.TryParse(input, out choice))
                {
                    Console.WriteLine(errMessage);
                    choice = -1;    // choice = -1 is sentinal
                }

                // Make sure it's within the correct bounds
                if ((choice <= 0 || choice > menuItems.Count + 1) && choice != -1)
                {
                    Console.WriteLine("{0}({1} to {2})", errMessage, 1, menuItems.Count + 1);
                    choice = -1;
                }

            } while (choice == -1);

            return (choice - 1 == menuItems.Count? QuitOption : menuItems[choice - 1]);
        }
    }

    public class MenuItem : IComparable<MenuItem>
    {
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        
        private int number;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        private int depth;
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public delegate void MethodDelegate();

        private MethodDelegate method;
        public MethodDelegate ItemMethodDelegate
        {
            get { return method; }
            set { method = value; }
        }


        public MenuItem()
        {
            Text = "null";
            number = 0;
            depth = 5;

            method = null;
        }

        public MenuItem(int number, string text, MethodDelegate method = null, int depth = 5)
        {
            Number = number;
            Text = text;
            Depth = depth;
            this.method = method;
        }

        public void DisplayMenuItem(int? number = null, int? depth = null)
        {
            // Make sure the parameters have values, Use the stored values if not
            if (!number.HasValue)
                number = Number;
            if (!depth.HasValue)
                depth = Depth;

            // Create a stringbuilder with the specified depth
            StringBuilder sb = new StringBuilder(new string(' ', depth.Value));

            // place the number in a justified field and append it
            sb.AppendFormat("{0, 2}. ", number.Value + 1);

            // Append the text and then print it
            sb.Append(text);
            Console.WriteLine(sb.ToString());

        }

        public void Execute()
        {
            if (method != null)
                method();
        }

        public int CompareTo(MenuItem other)
        {
            return this.number.CompareTo(other.number);
        }

        public override int GetHashCode()
        {
            return text.GetHashCode() ^ number ^ depth;
        }

        public static bool operator ==(MenuItem lhs, MenuItem rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MenuItem lhs, MenuItem rhs)
        {
            return !lhs.Equals(rhs);
        }

        public bool Equals(MenuItem other)
        {
            return text == other.text && number == other.number && depth == other.depth;
        }

        public override bool Equals(Object o)
        {
            if (o is MenuItem)
                return this.Equals(o);
            else
                throw new ArgumentException("Object not of type MenuItem!");
        }

        public override string ToString()
        {
            return String.Format("Number: {0}, Text: {1}, Depth: {2}, Method: {3}",
                    number, text, depth, (method != null? "true" : "false"));
        }

        static public void Debug()
        {
            Console.WriteLine("Testing MenuItem...");
            Console.WriteLine("Testing constructor...");
            MenuItem item = new MenuItem();

            bool isGood = item.Text == "null" && item.Number == 0 && item.Depth == 5;
            Console.WriteLine("No-args constructor is {0}good",
                    (isGood? " " : "not "));

            item = new MenuItem(3, "Second Test Item");

            isGood = item.Text == "Second Test Item" && item.Number == 3 && item.Depth == 5;
            Console.WriteLine("Constructor is {0}good",
                    (isGood? " " : "not "));

            Console.WriteLine("Constructor tests finished!");
            Console.WriteLine("Testing DisplayMenuItem...");
            Console.WriteLine("DisplayMenuItem()");
            item.DisplayMenuItem();
            Console.WriteLine("DisplayMenuItem(14, 22) | The number should start at the pipe.");
            item.DisplayMenuItem(14, 22);
            Console.WriteLine("DisplayMenuItem tests finished!");

            Console.WriteLine("All tests finished!");
        }
    }
}
