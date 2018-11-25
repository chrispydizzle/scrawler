namespace Scrawler
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Engine;
    using Engine.Output;
    using Engine.Results;

    internal class Program
    {
        /// <summary>
        /// Application entry point, takes one param - the url to begin crawling from
        /// </summary>
        /// <param name="args">argument one - the origin url to crawl</param>
        private static void Main(string[] args)
        {
            string initialUrl;
            int threads = 4;

            if (!Debugger.IsAttached)
            {
                if (!ValidateArgs(args))
                {
                    OutputHelp();
                    return;
                }

                initialUrl = args[0];

                if (args.Length > 1)
                {
                    threads = int.Parse(args[1]);
                }
            }
            else
            {
                Console.Write("Enter a domain to crawl: ");
                initialUrl = Console.ReadLine();
            }

            ScrawlerCrawler sc = new ScrawlerCrawler(new CrawlConfiguration() { Threads = threads });
            CrawlResult crawlResult = sc.Crawl(initialUrl);
            string result = new CrawlResultXmlWriter().Write(crawlResult);
            string domain = new Uri(crawlResult.BaseUrl).Authority;
            string pathLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string outputPath = $"{pathLocation}\\{domain}.crawlResult.xml";

            Console.WriteLine($"Writing results to {outputPath}");
            File.WriteAllText(outputPath, result);
            if (Debugger.IsAttached)
            {
                Main(args);
            }
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
            Console.WriteLine("An optional second argument, the number of threads the process should use to engage in crawling. (default is four)");
            Console.WriteLine("eg: scrawler cpsharp.net ..");
        }
    }
}