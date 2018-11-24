namespace Scrawler.Testing
{
    using Crawler;
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
    }
}