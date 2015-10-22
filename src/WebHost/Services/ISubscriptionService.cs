using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Services
{
    public interface ISubscriptionService
    {
        void OnMessages(Action<string> callback);
    }
}