using System;

namespace CoinGenerator
{
    public class CoinGenerator
    {
        public static void Main(string[] args)
        {
            CoinGenerator gen = new CoinGenerator();

            for(int i = 0; i < 20; i++)
                Console.WriteLine(gen.GenerateCoins(100));
        }

        static public Random rand = null;

        public double GPPercent { get; set;}
        public double SPPercent { get; set;}
        public double CPPercent { get; set;}
        public double FuzzyPercent { get; set;}
        public double GPWeight { get; set;}
        public double SPWeight { get; set;}
        public double CPWeight { get; set;}

        public CoinGenerator()
        {
            if (rand == null) rand = new Random();

            GPPercent = 0.7;
            SPPercent = 0.25;
            CPPercent = 0.05;

            FuzzyPercent = 0.05;
        }

        public CoinResult GenerateCoins(int worth)
        {
            double gPercent = GPPercent,
                   sPercent = SPPercent,
                   cPercent = CPPercent;

            //Console.WriteLine("gPercent: {0:0.00}", gPercent);
            //Console.WriteLine("sPercent: {0:0.00}", sPercent);
            //Console.WriteLine("cPercent: {0:0.00}", cPercent);

            // Generate a number between -FuzzyPercent and FuzzyPercent
            gPercent += rand.NextDouble() * 2 * FuzzyPercent - FuzzyPercent;
            sPercent += rand.NextDouble() * 2 * FuzzyPercent - FuzzyPercent;
            cPercent += rand.NextDouble() * 2 * FuzzyPercent - FuzzyPercent;
            
            //Console.WriteLine("gPercent: {0:0.00}", gPercent);
            //Console.WriteLine("sPercent: {0:0.00}", sPercent);
            //Console.WriteLine("cPercent: {0:0.00}", cPercent);

            double totalPercent = gPercent + sPercent + cPercent;

            //Console.WriteLine("total Percent: {0:0.00}", totalPercent);

            gPercent /= totalPercent;
            sPercent /= totalPercent;
            cPercent /= totalPercent;

            //Console.WriteLine("gPercent: {0:0.00}", gPercent);
            //Console.WriteLine("sPercent: {0:0.00}", sPercent);
            //Console.WriteLine("cPercent: {0:0.00}", cPercent);

            return new CoinResult((int)(worth * gPercent),
                    (int)(10 * worth * sPercent),
                    (int)(100 * worth * cPercent));
        }
    }

    public class CoinResult
    {
        private double goldWeight = 0.02;
        private double silverWeight = 0.02;
        private double copperWeight = 0.02;

        private int gold;
        private int silver;
        private int copper;

        public int GoldPieces { get {return gold;}}
        public int SilverPieces { get {return silver;}}
        public int CopperPieces { get {return copper;}}
        
        public double Worth { get {return getWorth();} }
        public double Weight { get {return getWeight(); } }

        public CoinResult(int gold, int silver, int copper)
        {
            this.gold = gold;
            this.silver = silver;
            this.copper = copper;
        }

        private double getWorth()
        {
            double worth = 0.0;
            worth += 1.0 * gold;
            worth += 0.10 * silver;
            worth += 0.01 * copper;

            return worth;
        }

        private double getWeight()
        {
            double weight = 0.0;
            weight += (double)gold * goldWeight;
            weight += (double)silver * silverWeight;
            weight += (double)copper * copperWeight;

            return weight;
        }

        public override string ToString()
        {
            return String.Format(
                    "A pile of coins worth {0} GP and weighing {1} lbs. ({2} GP, {3} SP, {4} CP)",
                    Worth, Weight, GoldPieces, SilverPieces, CopperPieces);
        }
    }
}
