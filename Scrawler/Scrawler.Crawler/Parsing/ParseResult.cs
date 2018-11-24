namespace Scrawler.Crawler
{
    using System.Collections.Generic;

    public class ParseResult
    {
        public ParseResult()
        {
            this.PageLinks = new HashSet<string>();
            this.StaticLinks = new HashSet<string>();
        }

        public HashSet<string> PageLinks { get; set; }

        public HashSet<string> StaticLinks { get; set; }
    }
}