namespace Scrawler.Crawler
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using Logging;
    using PageRequester;
    using Results;

    /// <summary>
    ///     The primary crawler class
    /// </summary>
    public class ScrawlerCrawler
    {
        private readonly ILog logger;
        private readonly IRequestPages pageRequester;
        private int maxDepth;

        /// <summary>
        ///     default ctor for concrete usage
        /// </summary>
        public ScrawlerCrawler()
            : this(new InternetPageRequester(), new ConsoleLogger(), 5)
        {
        }

        public ScrawlerCrawler(int maxDepth)
            : this(new InternetPageRequester(), new ConsoleLogger(), maxDepth)
        {
        }

        /// <summary>
        ///     ctor to pass in a page requester and use a standard console logger
        /// </summary>
        /// <param name="pageRequester">A page requester</param>
        public ScrawlerCrawler(IRequestPages pageRequester)
            : this(pageRequester, new ConsoleLogger(), 5)
        {
        }

        /// <summary>
        ///     ctor takes in a pagerequester so we can mock it for unit testing
        /// </summary>
        /// <param name="pageRequester">A page requester</param>
        /// <param name="logger">A logger</param>
        public ScrawlerCrawler(IRequestPages pageRequester, ILog logger, int maxDepth)
        {
            this.pageRequester = pageRequester;
            this.logger = logger;
            this.maxDepth = maxDepth;
        }

        /// <summary>
        ///     Kicks off a crawler at the targeted URL
        /// </summary>
        /// <param name="originUrl">The URL or domain name to crawl</param>
        public CrawlResult Crawl(string originUrl)
        {
            this.logger.Log($"Beginning Crawl of {originUrl}");

            if (!originUrl.StartsWith("http")) originUrl = $"http://{originUrl}";

            // Get root domain
            Uri originUri = new Uri(originUrl);
            string domain = originUri.Authority;

            // initialize crawler queue, hashset, and output result
            CrawlResult cr = new CrawlResult(originUri);

            HashSet<Uri> visited = new HashSet<Uri>();
            ConcurrentQueue<Uri> toVisit = new ConcurrentQueue<Uri>();

            toVisit.Enqueue(originUri);

            int depthCounter = 0;
            HashSet<Uri> nextLevel = new HashSet<Uri>();
            while (toVisit.Any() && depthCounter < 5)
            {
                if (!toVisit.TryDequeue(out Uri currentTarget))
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (visited.Contains(currentTarget)) continue;
                visited.Add(currentTarget);
                DomainResultNode resultNode = new DomainResultNode(currentTarget.AbsoluteUri);

                HttpResponseMessage responseMessage = this.pageRequester.Request(currentTarget);

                int statusCode = (int) responseMessage.StatusCode;
                this.logger.Log($"GET {currentTarget} - {statusCode}");

                resultNode.StatusCode = statusCode;
                if (statusCode == 200 && responseMessage.Content.Headers.ContentType.MediaType.StartsWith("text"))
                {
                    try
                    {
                        string body = responseMessage.Content.ReadAsStringAsync().Result;

                        ResultParser parser = new ResultParser(currentTarget);
                        ParseResult urls = parser.ParseResult(body);

                        foreach (string link in urls.PageLinks)
                        {
                            Uri currentUri = new Uri(link);
                            if (currentUri.Authority == domain && (currentUri.Scheme == "http" || currentUri.Scheme == "https"))
                            {
                                resultNode.InternalPageLinks.Add(new DomainResultLeaf(currentUri.AbsoluteUri));
                                nextLevel.Add(currentUri);
                            }
                            else
                            {
                                resultNode.ExternalPageLinks.Add(new ExternalResultLeaf(currentUri.AbsoluteUri));
                            }
                        }

                        foreach (string link in urls.StaticLinks)
                        {
                            Uri currentUri = new Uri(link);
                            if (currentUri.Authority == domain)
                            {
                                resultNode.InternalStaticLinks.Add(new DomainResultLeaf(currentUri.AbsoluteUri));
                            }
                            else
                            {
                                resultNode.ExternalStaticLinks.Add(new ExternalResultLeaf(currentUri.AbsoluteUri));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.Log($"An error occurred parsing {currentTarget}: {ex}");
                        resultNode.StatusCode = 500;
                    }

                    cr.TravesedPages.Add(resultNode);

                    if (!toVisit.Any())
                    {
                        toVisit = new ConcurrentQueue<Uri>(nextLevel);
                        this.logger.Log($"Increasing Depth to {depthCounter}");
                        depthCounter++;
                        nextLevel.Clear();
                    }
                }
            }

            return cr;
        }
    }
}