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
        void OnSensorStateChanged(Action<SensorStateChanges> callback);

        void OnSensorStateChangedAsync(Func<SensorStateChanges, Task> callback);
    }
}