namespace Scrawler.Crawler
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     A helper class for working with HTML. There are libraries out there that do this much better,
    ///     but in the interest of keeping my dependencies low and just-my-code, I did some regex magic to parse the
    ///     probably important strings.
    /// </summary>
    public class ResultParser
    {
        // This beast of a regex does all my parsing in a single pass
        private readonly Regex captureRegex = new Regex("(?:(?:(?:<link [^>]*href=['\"])([^'\"]*)['\"])|(?:<(?:[^</!>]+) [^>]* src=['\"]?([^'\" ]*)['\"]?)|(?:url\\(['\"]?([^'\")]*)['\"]?\\))|(?:<a [^>]*href=['\"]([^'\"#]*)))");
        private readonly Uri originUri;

        public ResultParser(Uri currentTarget)
        {
            this.originUri = currentTarget;
        }

        public ParseResult ParseResult(string body)
        {
            MatchCollection matchCollection = this.captureRegex.Matches(body);
            ParseResult pr = new ParseResult();

            foreach (Match match in matchCollection)
            {
                if (!string.IsNullOrEmpty(match.Groups[4].Value))
                {
                    string massageMatch = this.MassageMatch(match.Groups[4].Value);
                    if (!string.IsNullOrEmpty(massageMatch)) pr.PageLinks.Add(massageMatch);
                }
                else
                {
                    string massageMatch = this.MassageMatch(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                    if (!string.IsNullOrEmpty(massageMatch)) pr.StaticLinks.Add(massageMatch);
                }
            }

            return pr;
        }

        private string MassageMatch(params string[] args)
        {
            foreach (string s in args)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (Uri.TryCreate(s, UriKind.Absolute, out Uri output)) return s;

                    if (s.StartsWith("//"))
                    {
                        string newString = $"{this.originUri.Scheme}:{s}";
                        bool isWellFormedUriString = Uri.IsWellFormedUriString(newString, UriKind.Absolute);
                        if (isWellFormedUriString) return newString;
                    }

                    if (s.StartsWith("/"))
                    {
                        string newString = $"{this.originUri.Scheme}://{this.originUri.Authority}{s}";
                        bool isWellFormedUriString = Uri.IsWellFormedUriString(newString, UriKind.Absolute);
                        if (isWellFormedUriString) return newString;
                    }

                    if (!s.StartsWith("https://") && !s.StartsWith("http://"))
                    {
                        // Attempt to build a relative link
                        StringBuilder b = new StringBuilder();
                        foreach (string originUriSegment in this.originUri.Segments)
                        {
                            if (originUriSegment.EndsWith("/")) b.Append(originUriSegment);
                        }

                        b.Append(s);

                        return $"{this.originUri.Scheme}://{this.originUri.Authority}{b}";
                    }
                }
            }

            return null;
        }
    }
}