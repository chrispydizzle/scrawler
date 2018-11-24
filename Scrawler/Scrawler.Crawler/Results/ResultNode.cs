namespace Scrawler.Crawler.Results
{
    public abstract class ResultNode
    {
        protected ResultNode(string url)
        {
            this.Url = url;
        }

        public string Url { get; set; }
    }
}