namespace Scrawler.Engine.Results
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class DomainResultNode : DomainResultLeaf
    {
        public DomainResultNode()
        {
        }

        public DomainResultNode(string url)
            : base(url)
        {
            this.InternalPageLinks = new List<DomainResultLeaf>();
            this.InternalStaticLinks = new List<DomainResultLeaf>();
            this.ExternalPageLinks = new List<ExternalResultLeaf>();
            this.ExternalStaticLinks = new List<ExternalResultLeaf>();
        }

        [XmlArrayItem("LinkTo")]
        public List<DomainResultLeaf> InternalPageLinks { get; set; }

        [XmlArrayItem("LinkTo")]
        public List<DomainResultLeaf> InternalStaticLinks { get; set; }

        [XmlArrayItem("LinkTo")]
        public List<ExternalResultLeaf> ExternalPageLinks { get; set; }

        [XmlArrayItem("LinkTo")]
        public List<ExternalResultLeaf> ExternalStaticLinks { get; set; }

        [XmlAttribute]
        public int StatusCode { get; set; }
    }
}