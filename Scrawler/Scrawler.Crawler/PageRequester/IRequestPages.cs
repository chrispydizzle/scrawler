namespace Scrawler.Crawler.PageRequester
{
    using System;
    using System.Net.Http;

    public interface IRequestPages
    {
        HttpResponseMessage Request(Uri targetUri);
    }
}