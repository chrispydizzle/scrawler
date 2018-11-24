namespace Scrawler.Testing
{
    using System.Net;
    using System.Net.Http;

    public static class TestHelpers
    {
        public static HttpResponseMessage OkResponse(string body)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(body);
            return response;
        }

        public static class Html
        {
            public const string WiproDigital = @"";
        }
    }
}