namespace Scrawler.Engine.PageRequester
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class InternetPageRequester : IRequestPages
    {
        public HttpResponseMessage Request(Uri targetUri)
        {
            HttpClient client  = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(new HttpMethod("GET"), targetUri);
            Task<HttpResponseMessage> sendAsync = client.SendAsync(message);
            return sendAsync.Result;
        }
    }
}