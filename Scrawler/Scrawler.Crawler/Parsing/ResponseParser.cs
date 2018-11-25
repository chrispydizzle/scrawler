namespace Scrawler.Engine.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HtmlAgilityPack;

    /// <summary>
    ///     A helper class for working with HTML.
    ///     I started writing crazy regexs to do all of the parsing myself but it became very time consiming very fast, so I'm
    ///     leaning on HtmlAgility for the heavy lifting.
    /// </summary>
    public class ResponseParser
    {
        private readonly HtmlDocument document;
        private readonly Uri originUri;

        public ResponseParser(Uri uri, string body)
        {
            this.originUri = uri;
            this.document = new HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };

            this.document.LoadHtml(body);
        }

        public List<string> GetLinks()
        {
            HtmlNodeCollection hrefs = this.document.DocumentNode.SelectNodes("//a[@href]");

            if (hrefs == null)
            {
                return new List<string>();
            }

            return hrefs.Select(href => this.NormalizeUrl(href.Attributes["href"].Value)).Distinct().Where(u => !string.IsNullOrEmpty(u)).ToList();
        }

        public List<string> GetReferences()
        {
            HtmlNodeCollection nodes = this.document.DocumentNode.SelectNodes("//*[@background or @lowsrc or @src or @href or @action]");

            if (nodes == null)
            {
                return new List<string>();
            }

            return nodes.SelectMany(n => new[]
            {
                this.ParseReference(n, "background"),
                this.ParseReference(n, "href"),
                this.ParseReference(n, "src"),
                this.ParseReference(n, "lowsrc"),
                this.ParseReference(n, "action")
            }).Where(n => n != null).Distinct().ToList();
        }

        private string ParseReference(HtmlNode node, string name)
        {
            HtmlAttribute attribute = node.Attributes[name];
            if (attribute == null)
            {
                return null;
            }

            // don't want href if it's not a link tag.
            if (name == "href" && node.Name != "link")
            {
                return null;
            }

            return this.NormalizeUrl(attribute.Value);
        }

        private string NormalizeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return this.originUri.AbsoluteUri;
            }

            int indexOf = url.IndexOf('#');
            if (indexOf == 0)
            {
                return this.originUri.AbsoluteUri;
            }

            if (indexOf > 0)
            {
                url = url.Substring(0, indexOf);
            }

            if (url.IndexOf("..") != -1 || url.StartsWith("/") || !url.StartsWith("http") || !url.StartsWith("https"))
            {
                url = new Uri(new Uri(this.originUri.AbsoluteUri), url).AbsoluteUri;
            }

            if (Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                Uri absoluteBaseUrl = new Uri(this.originUri.AbsoluteUri, UriKind.Absolute);
                return new Uri(absoluteBaseUrl, url).ToString();
            }

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                Uri uri = new Uri(url);
                bool schemaMatch;

                if (this.originUri.Scheme == "http" || this.originUri.Scheme == "https")
                {
                    schemaMatch = "http" == uri.Scheme || "https" == uri.Scheme;
                }
                else
                {
                    schemaMatch = this.originUri.Scheme == uri.Scheme;
                }

                if (schemaMatch)
                {
                    return new Uri(url, UriKind.Absolute).ToString();
                }
            }

            return null;
        }
    }
}