using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Logger
{
    public interface ILogger
    {
        void Log(string message);

        void Log(string message, params object[] args);
    }
}