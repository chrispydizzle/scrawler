namespace Scrawler
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Crawler;
    using Crawler.Output;
    using Crawler.Results;

    internal class Program
    {
        /// <summary>
        /// Application entry point, takes one param - the url to begin crawling from
        /// </summary>
        /// <param name="args">argument one - the origin url to crawl</param>
        private static void Main(string[] args)
        {
            string initialUrl;
            if (!Debugger.IsAttached)
            {
                if (!ValidateArgs(args))
                {
                    OutputHelp();
                    return;
                }

                initialUrl = args[0];
            }
            else
            {
                Console.Write("Enter a domain to crawl: ");
                initialUrl = Console.ReadLine();
            }

            ScrawlerCrawler sc = new ScrawlerCrawler();
            CrawlResult crawlResult = sc.Crawl(initialUrl);
            string result = new CrawlResultXmlWriter().Write(crawlResult);
            string domain = new Uri(crawlResult.BaseUrl).Authority;
            string pathLocation = args.Length > 1 ? args[1] : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
            Console.WriteLine("An optional argument, the directory to output the result of the crawl, may be provided.");
            Console.WriteLine("eg: scrawler cpsharp.net ..");
        }
    }
}