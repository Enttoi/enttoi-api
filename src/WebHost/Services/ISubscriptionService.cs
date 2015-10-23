using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebHost.Services
{
    public interface ISubscriptionService
    {
        void OnMessages(Action<string> callback);

        void OnMessagesAsync(Func<string, Task> callback);
    }
}