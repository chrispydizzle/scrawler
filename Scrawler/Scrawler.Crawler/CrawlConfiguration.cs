namespace Scrawler.Engine
{
    public class CrawlConfiguration
    {
        public int Threads { get; set; }

        public static CrawlConfiguration Default()
        {
            return new CrawlConfiguration {Threads = 4};
        }
    }
}