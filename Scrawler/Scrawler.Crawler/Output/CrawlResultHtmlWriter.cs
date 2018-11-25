namespace Scrawler.Engine.Output
{
    using System.Text;
    using Results;

    public class CrawlResultHtmlWriter
    {
        public string Write(CrawlResult crawlResult)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"<html><head><title>Results from crawl on {crawlResult.BaseUrl}</title></head><body>");
            builder.AppendLine($"<p>Results from crawl on <b>{crawlResult.BaseUrl}</b></p>");
            builder.AppendLine($"<hr />");
            foreach (DomainResultNode resultNode in crawlResult.TravesedPages)
            {
                builder.AppendLine("<div class='tPage'>");
                builder.AppendLine($"<div class='url'>{resultNode.Url}</div>");
                builder.AppendLine($"<div class='urlContents'><p>Links to {resultNode.InternalPageLinks.Count} unique internal pages</p><ul>");

                foreach (ResultNode node in resultNode.InternalPageLinks)
                {
                    builder.AppendLine($"<li>{node.Url}</li>");
                }

                builder.AppendLine($"</ul><p>Contains {resultNode.InternalStaticLinks.Count} internal static references</p><ul>");
                foreach (ResultNode node in resultNode.InternalStaticLinks)
                {
                    builder.AppendLine($"<li>{node.Url}</li>");
                }

                builder.AppendLine($"</ul><p>Links to {resultNode.ExternalPageLinks.Count} external pages</p><ul>");
                foreach (ResultNode node in resultNode.ExternalPageLinks)
                {
                    builder.AppendLine($"<li>{node.Url}</li>");
                }

                builder.AppendLine($"</ul><p>Contains {resultNode.ExternalStaticLinks.Count} external static references</p><ul>");
                foreach (ResultNode node in resultNode.ExternalStaticLinks)
                {
                    builder.AppendLine($"<li>{node.Url}</li>");
                }

                builder.AppendLine("</ul></div></div>");
            }

            builder.AppendLine("</body></html>");
            return builder.ToString();
        }
    }
}