namespace Scrawler.Testing
{
    using System.Collections.Generic;
    using System.Linq;
    using Engine.Parsing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParsingFunctionalTests
    {
        [TestMethod]
        public void ParserGetsLinks()
        {
            string content = TestHelpers.Html.WiproDigital();
            ResponseParser rp = new ResponseParser(TestHelpers.TestValues.WiproDomainUri, content);
            List<string> references = rp.GetReferences();
            List<string> links = rp.GetLinks();

            Assert.AreEqual(29, references.Count);
            Assert.AreEqual(38, links.Count);
        }

        [TestMethod]
        public void ParserHandlesDoubleBackslash()
        {
            string content = TestHelpers.Html.DoubleBackslash();
            ResponseParser rp = new ResponseParser(TestHelpers.TestValues.TestDomainUri, content);
            IEnumerable<string> links = rp.GetReferences();
            Assert.IsTrue(links.Contains(TestHelpers.TestValues.ExternalDomainUrl));
        }

        [TestMethod]
        public void ParserHandlesSingleSlash()
        {
            string content = TestHelpers.Html.SingleBackslash();
            ResponseParser rp = new ResponseParser(TestHelpers.TestValues.TestDomainUri, content);
            IEnumerable<string> links = rp.GetLinks();
            Assert.IsTrue(links.Contains("http://testdomain.com/internal/link.html"));
        }

        [TestMethod]
        public void ParserHandlesRelativePath()
        {
            string content = TestHelpers.Html.RelativeLinks();
            ResponseParser rp = new ResponseParser(TestHelpers.TestValues.TestDomainRelativeUri, content);
            IEnumerable<string> links = rp.GetLinks();
            Assert.IsTrue(links.Contains("http://testdomain.com/internal/link.html"));
        }

        [TestMethod]
        public void ParserHandlesAnchors()
        {
            string content = TestHelpers.Html.AnchorLink();
            ResponseParser rp = new ResponseParser(TestHelpers.TestValues.TestDomainUri, content);
            IEnumerable<string> links = rp.GetLinks();
            Assert.IsTrue(links.Contains("http://testdomain.com/internal/link.html"));
            Assert.AreEqual(1, links.Count());
        }

        [TestMethod]
        public void ParserCatchesStatic()
        {
            string content = TestHelpers.Html.DoubleBackslash();
            ResponseParser rp = new ResponseParser(TestHelpers.TestValues.TestDomainUri, content);
            Assert.AreEqual(0, rp.GetLinks().Count);
            Assert.AreEqual(1, rp.GetReferences().Count);
        }

        [TestMethod]
        public void ParserCatchesLink()
        {
            string content = TestHelpers.Html.SingleBackslash();
            ResponseParser rp = new ResponseParser(TestHelpers.TestValues.TestDomainUri, content);
            Assert.AreEqual(1, rp.GetLinks().Count);
            Assert.AreEqual(0, rp.GetReferences().Count);
        }
    }
}