using System;
using TreasureGenerator;
using CoinGenerator;
using ConsoleHelper;
using System.Collections.Generic;
using System.IO;

namespace DMTools
{
    public class Driver
    {
        private static string fileSuffix = ".dat";
        private static string currencyDenomination = "GP";

        private static string[] files = {
            "data/gems.dat",
            "data/art.dat"
        };

        private static Dictionary<string, TreasureGenerator.TreasureGenerator> generators;
        private static CoinGenerator.CoinGenerator coinGen;

        public static void Main(string[] args)
        {
            // Create the dictionary that will be used to hold the generators
            generators = new Dictionary<string, TreasureGenerator.TreasureGenerator>(files.Length);

            // Get the files and make generators for them
            for( int i = 0; i < files.Length; i++)
                generators.Add(stripFileName(files[i]), new TreasureGenerator.TreasureGenerator(files[i]));

            coinGen = new CoinGenerator.CoinGenerator();

            // Create the menu items for the treasure menu
            List<MenuItem> menuItems = new List<MenuItem>();
            int menuOrder = 0;
            menuItems.Add(new MenuItem(menuOrder++, "Generate a gem", generateGem));
            menuItems.Add(new MenuItem(menuOrder++, "Generate a treasure", generateArt));
            menuItems.Add(new MenuItem(menuOrder++, "Generate coins", generateCoins));
            
            // Create the generator menu
            Menu generatorMenu = new Menu(menuItems, "What would you like to generate?", "Return to main menu");

            // Create the menu items for the main menu
            menuItems.Clear();
            menuOrder = 0;

            menuItems.Add(new MenuItem(menuOrder++, "Treasure Generator", generatorMenu.RunMenu));

            // Create the main menu
            Menu mainMenu = new Menu(menuItems);


            // Run the main menu
            mainMenu.RunMenu(wait:false);
        }

        static private string stripFileName(string filename)
        {
            // Check to make sure the file ends in the correct file suffix
            if(!filename.EndsWith(fileSuffix))
                    throw new Exception(
                        String.Format("{0} is not of {1} type.", filename, fileSuffix));

            // Split the name on / to separate paths
            string[] splitName = filename.Split('/');

            // Strip off the suffix
            string result = splitName[splitName.Length - 1].Replace(fileSuffix, null);

            return result;
        }

        static public void generateCoins()
        {
            // Get the upper limit for the coins
            
            int upperLimit = Helper.GetIntFromUser(
                "Enter the upper limit in GP: ");
            
            // Generate the result using the upper limit
            CoinResult result = coinGen.GenerateCoins(upperLimit);

            // Print out the result
            Console.WriteLine(result);
        }

        static public void generateGem()
        {
            // Get the upper limit for the gem
            
            int upperLimit = Helper.GetIntFromUser(
                "Enter the upper limit in GP(or -1 for no limit): ");
            
            // Generate the result using the upper limit
            TreasureResult result = generators["gems"].GenerateTreasure(upperLimit);

            // Print out the result
            Console.WriteLine("A {0} worth {1} {2}", result.Description, result.Worth, currencyDenomination);
        }

        static public void generateArt()
        {
            // Get the upper limit for the art
            
            int upperLimit = Helper.GetIntFromUser(
                "Enter the upper limit in GP(or -1 for no limit): ");
            
            // Generate the result using the upper limit
            TreasureResult result = generators["art"].GenerateTreasure(upperLimit);

            // Print out the result
            Console.WriteLine("A {0} worth {1} {2}", result.Description, result.Worth, currencyDenomination);
        }
    }
}
