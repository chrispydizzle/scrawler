namespace Scrawler.Testing
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;

    public static class TestHelpers
    {
        public static HttpResponseMessage OkResponse(string body)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(body)};
            return response;
        }

        public static class TestValues
        {
            public const string ExternalDomainUrl = "http://externaldomain.com/link.css";
            public const string TestDomainUrl = "http://testdomain.com/";
            public const string TestDomain = "testdomain.com";

            public static Uri TestDomainRelativeUri = new Uri($"http://testdomain.com/internal/index.html");
            public static Uri TestDomainUri = new Uri(TestDomainUrl);
            public static Uri WiproDomainUri = new Uri("http://wiprodigital.com");
        }

        public static class Html
        {
            public static string LinksImagesLocalRemote()
            {
                return File.ReadAllText("Html/LinksImagesLocalRemote.html");
            }

            public static string DuplicateLinkOne()
            {
                return File.ReadAllText("Html/DuplicateLinkOne.html");
            }

            public static string Empty()
            {
                return File.ReadAllText("Html/Empty.html");
            }

            public static string DuplicateLinkTwo()
            {
                return File.ReadAllText("Html/DuplicateLinkTwo.html");
            }

            public static string SingleBackslash()
            {
                return File.ReadAllText("Html/SingleBackslash.html");
            }

            public static string SomeLinks()
            {
                return File.ReadAllText("Html/SomeLinks.html");
            }

            public static string RelativeLinks()
            {
                return File.ReadAllText("Html/RelativeLinks.html");
            }

            public static string DoubleBackslash()
            {
                return File.ReadAllText("Html/DoubleBackslash.html");
            }

            public static string AnchorLink()
            {
                return File.ReadAllText("Html/AnchorLinks.html");
            }
        }
    }
}