namespace Scrawler.Crawler.Logging
{
    using System;

    public class ConsoleLogger : ILog
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}