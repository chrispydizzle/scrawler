namespace Scrawler.Testing.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Crawler.PageRequester;

    /// <summary>
    ///     A mock page requester we can pass in for testing
    /// </summary>
    internal class MockPageRequester : IRequestPages
    {
        public MockPageRequester()
        {
            this.Responses = new Dictionary<string, HttpResponseMessage>();
            this.Requests = new List<string>();
        }

        /// <summary>
        /// A map of urls & responses to return from the mock requester
        /// </summary>
        public Dictionary<string, HttpResponseMessage> Responses { get; }

        /// <summary>
        /// This list will contain all requests sent to the mock during the test run.
        /// </summary>
        public List<string> Requests { get; }

        public HttpResponseMessage Request(Uri targetUri)
        {
            string uriKey = targetUri.AbsoluteUri;

            this.Requests.Add(uriKey);

            if (!this.Responses.ContainsKey(uriKey))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return this.Responses[uriKey];
        }
    }
}