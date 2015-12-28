using Core.Models;
using System;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ISubscriptionService
    {
        void OnSensorStateChanged(Action<SensorStateMessage> callback);

        void OnSensorStateChangedAsync(Func<SensorStateMessage, Task> callback);
    }
}