namespace Scrawler.Testing.Mocks
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Crawler.Logging;

    internal class MockLogger : ILog
    {
        public MockLogger()
        {
            this.Logs = new List<string>();
        }

        public List<string> Logs { get; }

        public void Log(string message)
        {
            Trace.WriteLine(message);
            this.Logs.Add(message);
        }
    }
}