namespace Scrawler.Testing
{
    using System.Linq;
    using Engine;
    using Engine.Results;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public class CrawlingTests
    {
        [TestMethod]
        public void CrawlerAppendsHttpToUri()
        {
            MockPageRequester mpr = new MockPageRequester();
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr);
            sc.Crawl(TestHelpers.TestValues.TestDomain);
            Assert.IsTrue(mpr.Requests.Contains(TestHelpers.TestValues.TestDomainUrl));
        }

        [TestMethod]
        public void CrawlerDoesNotFollowExternalLinks()
        {
            MockPageRequester mpr = new MockPageRequester();
            mpr.Responses.Add("http://dogeplanet.com/", TestHelpers.OkResponse(TestHelpers.Html.DoubleBackslash()));
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr);
            sc.Crawl("http://dogeplanet.com/");
            Assert.AreEqual(1, mpr.Requests.Count);
        }

        [TestMethod]
        public void CrawlerFollowsInternalLinks()
        {
            MockPageRequester mpr = new MockPageRequester();
            mpr.Responses.Add("http://dogeplanet.com/", TestHelpers.OkResponse(TestHelpers.Html.DuplicateLinkOne()));
            mpr.Responses.Add("http://dogeplanet.com/DuplicateLinkTwo.html", TestHelpers.OkResponse(TestHelpers.Html.DuplicateLinkTwo()));
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr);
            sc.Crawl("http://dogeplanet.com/");
            Assert.IsTrue(mpr.Requests.Contains("http://dogeplanet.com/thisisnotadupe.html"));
        }

        [TestMethod]
        public void CrawlerIgnoresDupes()
        {
            MockPageRequester mpr = new MockPageRequester();
            mpr.Responses.Add("http://dogeplanet.com/", TestHelpers.OkResponse(TestHelpers.Html.DuplicateLinkOne()));
            mpr.Responses.Add("http://dogeplanet.com/DuplicateLinkTwo.html", TestHelpers.OkResponse(TestHelpers.Html.DuplicateLinkTwo()));
            mpr.Responses.Add("http://dogeplanet.com/thisisadupe.html", TestHelpers.OkResponse(TestHelpers.Html.Empty()));
            mpr.Responses.Add("http://dogeplanet.com/thisisnotadupe.html", TestHelpers.OkResponse(TestHelpers.Html.Empty()));
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr);
            sc.Crawl("http://dogeplanet.com/");
            Assert.AreEqual(4, mpr.Requests.Count);
        }

        [TestMethod]
        public void CrawlerRespectsThreadLimit()
        {
            MockLogger ml = new MockLogger();
            MockPageRequester mpr = new MockPageRequester();
            mpr.Responses.Add("http://dogeplanet.com/", TestHelpers.OkResponse(TestHelpers.Html.SomeLinks()));
            mpr.ResponseDelay = 500;
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr, ml, new CrawlConfiguration() { Threads = 6 });
            sc.Crawl("http://dogeplanet.com/");
            Assert.IsTrue(ml.Logs.Any(s => s.Contains("Crawler reached thread limit")));
        }

        [TestMethod]
        public void CrawlerWithoutThreadLimit()
        {
            MockLogger ml = new MockLogger();
            MockPageRequester mpr = new MockPageRequester();
            mpr.Responses.Add("http://dogeplanet.com/", TestHelpers.OkResponse(TestHelpers.Html.SomeLinks()));
            ScrawlerCrawler sc = new ScrawlerCrawler(mpr, ml, new CrawlConfiguration() { Threads = 100 });
            sc.Crawl("http://dogeplanet.com/");
            Assert.IsFalse(ml.Logs.Any(s => s.Contains("Crawler reached thread limit")));
        }
    }
}