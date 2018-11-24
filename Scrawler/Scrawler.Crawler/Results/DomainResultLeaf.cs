namespace Scrawler.Crawler.Results
{
    using System.Xml.Serialization;

    public class DomainResultLeaf : ResultNode
    {
        public DomainResultLeaf()
        {
        }

        public DomainResultLeaf(string url)
            : base(url)
        {
        }
    }
}