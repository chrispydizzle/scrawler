namespace Scrawler.Engine.Results
{
    public class ExternalResultLeaf : ResultNode
    {
        public ExternalResultLeaf()
        {
        }

        public ExternalResultLeaf(string url)
            : base(url)
        {
        }
    }
}