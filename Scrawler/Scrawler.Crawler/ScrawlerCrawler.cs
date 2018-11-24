namespace Scrawler.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Logging;
    using PageRequester;
    using Results;

    /// <summary>
    ///     This guy handles initing the crawler and
    /// </summary>
    public class ScrawlerCrawler
    {
        private readonly ILog logger;
        private readonly IRequestPages pageRequester;
        private readonly ResultParser parser;

        /// <summary>
        ///     default ctor for concrete usage
        /// </summary>
        public ScrawlerCrawler()
            : this(new InternetPageRequester(), new ConsoleLogger())
        {
        }

        public ScrawlerCrawler(IRequestPages pageRequester)
        : this(pageRequester, new ConsoleLogger())
        {
        }

        /// <summary>
        ///     ctor takes in a pagerequester so we can mock it for unit testing
        /// </summary>
        /// <param name="pageRequester">A page requester</param>
        /// <param name="logger">A logger</param>
        public ScrawlerCrawler(IRequestPages pageRequester, ILog logger)
        {
            this.pageRequester = pageRequester;
            this.logger = logger;
            this.parser = new ResultParser();
        }

        /// <summary>
        ///     Kicks off a crawler at the targeted URL
        /// </summary>
        /// <param name="originUrl">The URL to crawl</param>
        public CrawlResult Crawl(string originUrl)
        {
            this.logger.Log($"Beginning Crawl of {originUrl}");

            if (!originUrl.StartsWith("http://"))
            {
                originUrl = $"http://{originUrl}";
            }

            // Get root domain
            Uri originUri = new Uri(originUrl);
            string domain = originUri.Authority;

            // initialize crawler queue, hashset, and output result
            CrawlResult cr = new CrawlResult(domain);
            HashSet<string> visited = new HashSet<string>();
            Queue<string> toVisit = new Queue<string>();

            toVisit.Enqueue(originUrl);

            while (toVisit.Any())
            {
                string currentTarget = toVisit.Dequeue();

                // don't revisit visited urls
                if (visited.Contains(currentTarget)) continue;

                this.logger.Log($"Evaluating {currentTarget}");
                visited.Add(currentTarget);

                DomainResultNode resultNode = new DomainResultNode(currentTarget);

                Uri targetUri = new Uri(currentTarget);
                HttpResponseMessage responseMessage = this.pageRequester.Request(targetUri);

                int statusCode = (int)responseMessage.StatusCode;
                this.logger.Log($"GET {currentTarget} - {statusCode}");

                resultNode.StatusCode = statusCode;

                if (statusCode == 200)
                {
                    // Status code 200 is ok - we can check the content 
                    if (responseMessage.Content.Headers.ContentType.MediaType.StartsWith("text"))
                    {
                        string body = responseMessage.Content.ReadAsStringAsync().Result;
                        List<string> urls = this.parser.GetAnchorLinks(body);

                        foreach (string link in urls)
                        {
                            if (link.StartsWith(domain))
                            {
                                resultNode.DomainResultNodes.Add(new DomainResultNode(link));
                                toVisit.Enqueue(link);
                            }
                            else
                            {
                                resultNode.ExternalResultLeaves.Add(new ExternalResultLeaf(link));
                            }
                        }

                        List<string> staticLinks = this.parser.GetStaticLinks(body);
                        foreach (string link in staticLinks)
                        {
                            if (link.StartsWith(domain))
                            {
                                resultNode.DomainResultLeaves.Add(new DomainResultLeaf(link));
                            }
                            else
                            {
                                resultNode.ExternalResultLeaves.Add(new ExternalResultLeaf(link));
                            }
                        }
                    }
                }

                cr.DomainResultNodes.Add(resultNode);
            }

            return cr;
        }
    }
}