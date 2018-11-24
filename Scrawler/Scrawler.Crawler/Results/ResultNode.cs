namespace Scrawler.Crawler.Results
{
    using System.Xml.Serialization;

    public abstract class ResultNode
    {
        protected ResultNode()
        {
        }

        protected ResultNode(string url)
        {
            this.Url = url;
        }

        [XmlAttribute]
        public string Url { get; set; }
    }
}