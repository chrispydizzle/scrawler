namespace Scrawler.Testing
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;

    public static class TestHelpers
    {
        public static class TestValues
        {
            public const string ExternalDomainUrl = "http://externaldomain.com/link.css";
            public const string TestDomainUrl = "http://testdomain.com";
            public const string TestDomain = "testdomain.com";
            public const string RelativeInternalLink = "/internal/index.html";
            public const string TrailingInternalLink = "/internal/link.html";
            
            public static Uri TestDomainRelativeUri = new Uri($"{TestDomainUrl}{RelativeInternalLink}");
            public static Uri TestDomainUri = new Uri(TestDomainUrl);
        }

        public static HttpResponseMessage OkResponse(string body)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(body)};
            return response;
        }

        public static class Html
        {
            public static string SingleBackslash()
            {
                return File.ReadAllText("Html/SingleBackslash.html");
            }

            public static string RelativeLinks()
            {
                return File.ReadAllText("Html/RelativeLinks.html");
            }

            public static string DoubleBackslash()
            {
                return File.ReadAllText("Html/DoubleBackslash.html");
            }

            public static string InvalidLink()
            {
                return File.ReadAllText("Html/InvalidLink.html");
            }

            public static string AnchorLink()
            {
                return File.ReadAllText("Html/AnchorLinks.html");
            }

            public static string WiproDigital()
            {
                return File.ReadAllText("Html/wiprodigital.html");
            }

            public static string InteractiveEdge()
            {
                return File.ReadAllText("Html/interactiveedge.html");
            }
        }
    }
}