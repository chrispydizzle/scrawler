namespace Scrawler.Crawler.Output
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Results;

    public class CrawlResultXmlWriter
    {
        public string Write(CrawlResult crawlResult)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlSerializer xs = new XmlSerializer(typeof(CrawlResult));
            xs.Serialize(writer, crawlResult);
            return sb.ToString();
        }
    }
}