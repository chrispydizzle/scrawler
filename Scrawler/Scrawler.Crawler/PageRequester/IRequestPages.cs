namespace Scrawler.Engine.PageRequester
{
    using System;
    using System.Net.Http;

    public interface IRequestPages
    {
        HttpResponseMessage Request(Uri targetUri);
    }
}