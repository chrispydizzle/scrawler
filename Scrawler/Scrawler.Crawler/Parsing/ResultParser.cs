namespace Scrawler.Crawler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Results;

    /// <summary>
    /// A helper class for working with HTML. There are libraries out there that do this much better,
    /// but in the interest of keeping 
    /// </summary>
    public class ResultParser
    {
        public List<string> GetStaticLinks(string body)
        {
            List<string> staticLinks = new List<string>();
            staticLinks.AddRange(this.GetLinkTags(body));
            staticLinks.AddRange(this.GetScriptTags(body));
            staticLinks.AddRange(this.GetImageTags(body));

            return staticLinks;
        }

        private List<string> GetMatchDistinctList(MatchCollection matches)
        {
            return (from Match match in matches select match.Groups into groups select groups[1].Value).Distinct().ToList();
        }

        private IEnumerable<string> GetImageTags(string body)
        {
            MatchCollection matches = Regex.Matches(body, "<img [^>]*src=\"([^\"]*)\"");
            return this.GetMatchDistinctList(matches);
        }

        private IEnumerable<string> GetScriptTags(string body)
        {
            MatchCollection matches = Regex.Matches(body, "<script [^>]*src=\"([^\"]*)\"");
            return this.GetMatchDistinctList(matches);
        }

        private IEnumerable<string> GetLinkTags(string body)
        {
            MatchCollection matches = Regex.Matches(body, "<link [^>]*href=\"([^\"]*)\"");
            return this.GetMatchDistinctList(matches);
        }

        public List<string> GetAnchorLinks(string body)
        {
            MatchCollection matches = Regex.Matches(body, "<a [^>]*href=\"([^\"#]*)");
            return this.GetMatchDistinctList(matches);
        }
    }
}