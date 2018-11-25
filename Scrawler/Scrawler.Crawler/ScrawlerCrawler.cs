namespace Scrawler.Engine
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using Logging;
    using PageRequester;
    using Parsing;
    using Results;

    /// <summary>
    ///     The primary crawler class
    /// </summary>
    public class ScrawlerCrawler
    {
        private readonly CrawlConfiguration config;
        private readonly ILog logger;
        private readonly IRequestPages pageRequester;
        private readonly CountdownEvent threadLimiter = new CountdownEvent(1);

        /// <summary>
        ///     default ctor for concrete usage
        /// </summary>
        public ScrawlerCrawler()
            : this(new InternetPageRequester(), new ConsoleLogger())
        {
        }

        public ScrawlerCrawler(CrawlConfiguration config)
            : this(new InternetPageRequester(), new ConsoleLogger(), config)
        {
        }

        /// <summary>
        ///     ctor to pass in a page requester, use a standard console logger, with infinite depth
        /// </summary>
        /// <param name="pageRequester">A page requester</param>
        public ScrawlerCrawler(IRequestPages pageRequester)
            : this(pageRequester, new ConsoleLogger())
        {
        }

        /// <summary>
        ///     ctor takes in a pagerequester so we can mock it for unit testing
        /// </summary>
        /// <param name="pageRequester">A page requester</param>
        /// <param name="logger">A logger</param>
        public ScrawlerCrawler(IRequestPages pageRequester, ILog logger, CrawlConfiguration config = null)
        {
            this.pageRequester = pageRequester;
            this.logger = logger;
            this.config = config ?? CrawlConfiguration.Default();
        }

        /// <summary>
        ///     Kicks off a crawler at the targeted URL
        /// </summary>
        /// <param name="originUrl">The URL or domain name to crawl</param>
        public CrawlResult Crawl(string originUrl)
        {
            this.logger.Log($"Beginning Crawl of {originUrl}");

            if (!originUrl.StartsWith("http"))
            {
                originUrl = $"http://{originUrl}";
            }

            Uri originUri = new Uri(originUrl);

            HashSet<Uri> visitedUris = new HashSet<Uri>();
            ConcurrentQueue<Uri> foundUris = new ConcurrentQueue<Uri>();
            ConcurrentBag<DomainResultNode> pageNodes = new ConcurrentBag<DomainResultNode>();

            foundUris.Enqueue(originUri);

            bool throttling = false;

            // crawl as long as the queue is non-empty or we have active threads.
            while (foundUris.TryDequeue(out Uri currentTarget) || this.threadLimiter.CurrentCount > 1)
            {
                // kick out duplicates and nulls
                if (visitedUris.Contains(currentTarget) || currentTarget == null)
                {
                    continue;
                }

                // re-queue an item if we're over our thread count.
                if (this.threadLimiter.CurrentCount > this.config.Threads)
                {
                    if (!throttling)
                    {
                        this.logger.Log($"Crawler reached thread limit {this.config.Threads}. Throttling.");
                        throttling = true;
                    }

                    foundUris.Enqueue(currentTarget);
                    continue;
                }

                throttling = false;

                // start work on the next link and boost the limiter
                visitedUris.Add(currentTarget);
                this.threadLimiter.AddCount();

                ThreadPool.QueueUserWorkItem(state =>
                {
                    Uri target = (Uri) state;

                    try
                    {
                        this.logger.Log($"Crawler processing {target}.");
                        ResponseResult processedResponse = new ResponseWorker(target).ProcessResponse(this.pageRequester.Request(target));

                        foreach (Uri uri in processedResponse.InternalUris)
                        {
                            foundUris.Enqueue(uri);
                        }

                        pageNodes.Add(processedResponse.DomainResultNode);
                    }
                    catch (Exception ex)
                    {
                        this.logger.Log($"An error occurred parsing {state}: {ex}");
                    }
                    finally
                    {
                        // always release thread limiit
                        this.threadLimiter.Signal();
                    }
                }, currentTarget);
            }

            // release initial thread limit and wait for the remainder of the threads
            this.threadLimiter.Signal();
            this.threadLimiter.Wait();

            return new CrawlResult(originUri, pageNodes);
        }
    }
}