namespace Scrawler.Crawler.Results
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class CrawlResult
    {
        public CrawlResult()
        {
        }

        public CrawlResult(Uri uri)
        {
            this.BaseUrl = uri.AbsoluteUri;
            this.Domain = uri.Authority;
            this.TravesedPages = new List<DomainResultNode>();
        }

        [XmlAttribute]
        public string Domain { get; set; }

        [XmlAttribute]
        public string BaseUrl { get; set; }

        [XmlArrayItem("Page")]
        public List<DomainResultNode> TravesedPages { get; set; }
    }
}