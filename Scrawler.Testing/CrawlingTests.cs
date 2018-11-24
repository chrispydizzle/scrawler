namespace Scrawler.Testing
{
    using System.IO;
    using Crawler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public class CrawlingTests
    {
        [TestMethod]
        public void TestCrawlerIgnoresMailtos()
        {
        }

        [TestMethod]
        public void TestCrawlerFollowsInternalLinks()
        {
        }

        [TestMethod]
        public void TestCrawlerRecognizesAnchors()
        {
        }

        [TestMethod]
        public void TestCrawlerIgnoresExternalLinks()
        {
        }

        [TestMethod]
        public void CrawlerHandles404()
        {
        }

        [TestMethod]
        public void TestWipro()
        {
            MockPageRequester mpr = new MockPageRequester();
            MockLogger ml = new MockLogger();
            mpr.Responses.Add("http://wiprodigital.com/", TestHelpers.OkResponse(File.ReadAllText("Html/wiprodigital.html")));
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr, ml);
            sc.Crawl("http://wiprodigital.com/");

            Assert.IsTrue(mpr.Requests.Contains("http://wiprodigital.com/"));
        }

        [TestMethod]
        public void CrawlerAppendsHttpToUri()
        {
            MockPageRequester mpr = new MockPageRequester();
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr);
            sc.Crawl("wiprodigital.com");

            Assert.IsTrue(mpr.Requests.Contains("http://wiprodigital.com/"));
        }
    }
}