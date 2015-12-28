using Core;
using System.Diagnostics;

namespace WebHost.Logger
{
    public class TraceLogger : ILogger
    {
        public void Log(string message)
        {
            Trace.TraceInformation(message);
        }

        public void Log(string message, params object[] args)
        {
            Trace.TraceInformation(message, args);
        }
    }
}