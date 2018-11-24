namespace Scrawler.Crawler.Results
{
    using System.Collections.Generic;

    public class DomainResultNode : DomainResultLeaf
    {
        public DomainResultNode(string url)
            : base(url)
        {
            this.DomainResultNodes = new List<DomainResultNode>();
            this.DomainResultLeaves = new List<DomainResultLeaf>();
            this.ExternalResultLeaves = new List<ExternalResultLeaf>();
        }

        public List<DomainResultNode> DomainResultNodes { get; set; }

        public List<DomainResultLeaf> DomainResultLeaves { get; set; }

        public List<ExternalResultLeaf> ExternalResultLeaves { get; set; }
    }
}