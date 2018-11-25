namespace Scrawler.Testing
{
    using Engine.Parsing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResponseWorkerTests
    {
        [TestMethod]
        public void CrawlerAllocatesExternalStaticsCorrectly()
        {
            ResponseWorker worker = new ResponseWorker(TestHelpers.TestValues.TestDomainUri);
            ResponseResult processResponse = worker.ProcessResponse(TestHelpers.OkResponse(TestHelpers.Html.LinksImagesLocalRemote()));
            Assert.IsTrue(processResponse.DomainResultNode.ExternalStaticLinks.Count == 1);
            Assert.IsTrue(processResponse.DomainResultNode.ExternalStaticLinks[0].Url == "http://otherdomain.com/images/doge.jpg");
        }

        [TestMethod]
        public void CrawlerAllocatesInternalStaticsCorrectly()
        {
            ResponseWorker worker = new ResponseWorker(TestHelpers.TestValues.TestDomainUri);
            ResponseResult processResponse = worker.ProcessResponse(TestHelpers.OkResponse(TestHelpers.Html.LinksImagesLocalRemote()));
            Assert.IsTrue(processResponse.DomainResultNode.InternalStaticLinks.Count == 1);
            Assert.IsTrue(processResponse.DomainResultNode.InternalStaticLinks[0].Url == "http://testdomain.com/images/doge.jpg");
        }

        [TestMethod]
        public void CrawlerAllocatesExternalLinksCorrectly()
        {
            ResponseWorker worker = new ResponseWorker(TestHelpers.TestValues.TestDomainUri);
            ResponseResult processResponse = worker.ProcessResponse(TestHelpers.OkResponse(TestHelpers.Html.LinksImagesLocalRemote()));
            Assert.IsTrue(processResponse.DomainResultNode.ExternalPageLinks.Count == 1);
            Assert.IsTrue(processResponse.DomainResultNode.ExternalPageLinks[0].Url == "http://otherdomain/dogepictures/doge.htm");
        }

        [TestMethod]
        public void CrawlerAllocatesInternalLinksCorrectly()
        {
            ResponseWorker worker = new ResponseWorker(TestHelpers.TestValues.TestDomainUri);
            ResponseResult processResponse = worker.ProcessResponse(TestHelpers.OkResponse(TestHelpers.Html.LinksImagesLocalRemote()));
            Assert.IsTrue(processResponse.InternalUris.Count == 1);
            Assert.IsTrue(processResponse.InternalUris[0].AbsoluteUri == "http://testdomain.com/dogepictures/doge.htm");
            Assert.IsTrue(processResponse.DomainResultNode.InternalPageLinks.Count == 1);
            Assert.IsTrue(processResponse.DomainResultNode.InternalPageLinks[0].Url == "http://testdomain.com/dogepictures/doge.htm");
        }
    }
}