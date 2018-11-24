namespace Scrawler.Crawler.Output
{
    using System.Text;
    using Results;

    public class CrawlResultTextWriter
    {
        public string Write(CrawlResult crawlResult)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Results from crawl on {crawlResult.BaseUrl}");
            builder.AppendLine($"-----");
            foreach (DomainResultNode resultNode in crawlResult.TravesedPages)
            {
                builder.AppendLine($"Url: {resultNode.Url}");
                builder.AppendLine($"Links to {resultNode.InternalPageLinks.Count} unique internal pages");
                foreach (ResultNode node in resultNode.InternalPageLinks)
                {
                    builder.AppendLine($"--Url: {node.Url}");
                }

                builder.AppendLine($"Contains {resultNode.InternalStaticLinks.Count} internal static references");
                foreach (ResultNode node in resultNode.InternalStaticLinks)
                {
                    builder.AppendLine($"--Url: {node.Url}");
                }

                builder.AppendLine($"Links to {resultNode.ExternalPageLinks.Count} external pages");
                foreach (ResultNode node in resultNode.ExternalPageLinks)
                {
                    builder.AppendLine($"--Url: {node.Url}");
                }

                builder.AppendLine($"Contains {resultNode.ExternalStaticLinks.Count} external static references");
                foreach (ResultNode node in resultNode.ExternalStaticLinks)
                {
                    builder.AppendLine($"--Url: {node.Url}");
                }
            }

            return builder.ToString();
        }
    }
}