namespace Scrawler.Testing
{
    using Crawler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParsingFunctionalTests
    {
        [TestMethod]
        public void ParserHandlesDoubleBackslash()
        {
            string content = TestHelpers.Html.DoubleBackslash();
            ResultParser rp = new ResultParser(TestHelpers.TestValues.TestDomainUri);
            ParseResult parseResult = rp.ParseResult(content);
            Assert.IsTrue(parseResult.StaticLinks.Contains(TestHelpers.TestValues.ExternalDomainUrl));
        }

        [TestMethod]
        public void ParserHandlesSingleSlash()
        {
            string content = TestHelpers.Html.SingleBackslash();
            ResultParser rp = new ResultParser(TestHelpers.TestValues.TestDomainUri);
            ParseResult parseResult = rp.ParseResult(content);
            Assert.IsTrue(parseResult.PageLinks.Contains($"{TestHelpers.TestValues.TestDomainUrl}{TestHelpers.TestValues.TrailingInternalLink}"));
        }

        [TestMethod]
        public void ParserHandlesRelativePath()
        {
            string content = TestHelpers.Html.RelativeLinks();
            ResultParser rp = new ResultParser(TestHelpers.TestValues.TestDomainRelativeUri);
            ParseResult parseResult = rp.ParseResult(content);
            Assert.IsTrue(parseResult.PageLinks.Contains($"{TestHelpers.TestValues.TestDomainUrl}{TestHelpers.TestValues.TrailingInternalLink}"));
        }

        [TestMethod]
        public void ParserHandlesAnchors()
        {
            string content = TestHelpers.Html.AnchorLink();
            ResultParser rp = new ResultParser(TestHelpers.TestValues.TestDomainUri);
            ParseResult parseResult = rp.ParseResult(content);
            Assert.IsTrue(parseResult.PageLinks.Contains($"{TestHelpers.TestValues.TestDomainUrl}{TestHelpers.TestValues.TrailingInternalLink}"));
            Assert.AreEqual(1, parseResult.PageLinks.Count);
        }

        [TestMethod]
        public void ParserDiscardsInvalid()
        {
            string content = TestHelpers.Html.InvalidLink();
            ResultParser rp = new ResultParser(TestHelpers.TestValues.TestDomainUri);
            ParseResult parseResult = rp.ParseResult(content);
            Assert.AreEqual(0, parseResult.PageLinks.Count + parseResult.StaticLinks.Count);
        }

        [TestMethod]
        public void ParserCatchesStatic()
        {
            string content = TestHelpers.Html.DoubleBackslash();
            ResultParser rp = new ResultParser(TestHelpers.TestValues.TestDomainUri);
            ParseResult parseResult = rp.ParseResult(content);
            Assert.AreEqual(0, parseResult.PageLinks.Count);
            Assert.AreEqual(1, parseResult.StaticLinks.Count);
        }

        [TestMethod]
        public void ParserCatchesLink()
        {
            string content = TestHelpers.Html.SingleBackslash();
            ResultParser rp = new ResultParser(TestHelpers.TestValues.TestDomainUri);
            ParseResult parseResult = rp.ParseResult(content);
            Assert.AreEqual(0, parseResult.StaticLinks.Count);
            Assert.AreEqual(1, parseResult.PageLinks.Count);
        }
    }
}