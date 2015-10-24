using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHost.Models;

namespace WebHost.Services
{
    public interface ISubscriptionService
    {
        void OnSensorStateChanged(Action<SensorStateMessage> callback);

        void OnSensorStateChangedAsync(Func<SensorStateMessage, Task> callback);
    }
}