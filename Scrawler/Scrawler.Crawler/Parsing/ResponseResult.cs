namespace Scrawler.Engine.Parsing
{
    using System;
    using System.Collections.Generic;
    using Results;

    /// <summary>
    /// Represents the output of a ResponseWorker
    /// </summary>
    public class ResponseResult
    {
        public ResponseResult(Uri target)
        {
            this.DomainResultNode = new DomainResultNode(target.AbsoluteUri);
            this.InternalUris = new List<Uri>();
        }

        public List<Uri> InternalUris { get; set; }

        public DomainResultNode DomainResultNode { get; set; }
    }
}