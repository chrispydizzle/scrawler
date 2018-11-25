namespace Scrawler.Engine.Parsing
{
    using System;
    using System.Net.Http;
    using Results;


    /// <summary>
    /// This class is responsbile for feeding the http response to the html parser, and determining how the resultant links should be allocated within each node.
    /// </summary>
    public class ResponseWorker
    {
        private readonly Uri target;

        public ResponseWorker(Uri target)
        {
            this.target = target;
        }

        public ResponseResult ProcessResponse(HttpResponseMessage responseMessage)
        {
            ResponseResult result = new ResponseResult(this.target);
            int statusCode = (int)responseMessage.StatusCode;
            result.DomainResultNode.StatusCode = statusCode;

            if (statusCode == 200 && responseMessage.Content.Headers.ContentType.MediaType.StartsWith("text"))
            {
                string body = responseMessage.Content.ReadAsStringAsync().Result;

                ResponseParser parser = new ResponseParser(this.target, body);

                foreach (string link in parser.GetLinks())
                {
                    Uri currentUri = new Uri(link);
                    if (currentUri.Authority == this.target.Authority)
                    {
                        result.DomainResultNode.InternalPageLinks.Add(new DomainResultLeaf(currentUri.AbsoluteUri));
                        result.InternalUris.Add(currentUri);
                    }
                    else
                    {
                        result.DomainResultNode.ExternalPageLinks.Add(new ExternalResultLeaf(currentUri.AbsoluteUri));
                    }
                }

                foreach (string link in parser.GetReferences())
                {
                    Uri currentUri = new Uri(link);
                    if (currentUri.Authority == this.target.Authority)
                    {
                        result.DomainResultNode.InternalStaticLinks.Add(new DomainResultLeaf(currentUri.AbsoluteUri));
                    }
                    else
                    {
                        result.DomainResultNode.ExternalStaticLinks.Add(new ExternalResultLeaf(currentUri.AbsoluteUri));
                    }
                }
            }

            return result;
        }
    }
}