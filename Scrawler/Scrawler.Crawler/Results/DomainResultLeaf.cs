namespace Scrawler.Crawler.Results
{
    public class DomainResultLeaf : ResultNode
    {
        public DomainResultLeaf(string url)
            : base(url)
        {
        }

        public int StatusCode { get; set; }
    }
}