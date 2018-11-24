namespace Scrawler
{
    using System;
    using Crawler;

    internal class Program
    {
        /// <summary>
        /// Application entry point, takes one param - the url to begin crawling from
        /// </summary>
        /// <param name="args">argument one - the origin url to crawl</param>
        private static void Main(string[] args)
        {
            if (!ValidateArgs(args))
            {
                OutputHelp();
                return;
            }

            string initialUrl = args[0];

            ScrawlerCrawler sc = new ScrawlerCrawler();
            sc.Crawl(initialUrl);
        }

        /// <summary>
        /// Validate inputs
        /// </summary>
        /// <param name="args">Inputs to the console apps</param>
        /// <returns>Whether or not the input was valid</returns>
        private static bool ValidateArgs(string[] args)
        {
            if (args.Length == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Give the user some help.
        /// </summary>
        private static void OutputHelp()
        {
            Console.WriteLine("At least one argument, the origin url, should be passed in.");
        }
    }
}