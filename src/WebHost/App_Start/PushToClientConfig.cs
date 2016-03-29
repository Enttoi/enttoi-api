using Autofac;
using Core.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using WebHost.Hubs;
using WebHost.Models;

namespace WebHost
{
    internal static class PushToClientConfig
    {
        public static void Register(IContainer container, IDependencyResolver resolver)
        {
            var serviceBusService = container.Resolve<ISubscriptionService>();
            var counterService = container.Resolve<IDistributedCounter>();
            var hub = resolver.Resolve<IConnectionManager>().GetHubContext<CommonHub>();

            serviceBusService.OnSensorStateChangedAsync(async (state) =>
            {
                await hub.Clients.All.sensorStatePush(new SensorClientUpdate
                {
                    clientId = state.ClientId,
                    sensorId = state.sensorId,
                    sensorType = state.sensorType,
                    newState = state.NewState,
                    timestamp = state.Timestamp
                });
            });

            serviceBusService.OnClientStateChangedAsync(async (state) =>
            {
                await hub.Clients.All.clientStatePush(new ClientUpdate
                {
                    clientId = state.ClientId,
                    newState = state.NewState,
                    timestamp = state.Timestamp
                });
            });

            counterService.OnCounterChangedAsync(async (counter) => {
                await hub.Clients.All.onlineUsersPush(counter);
            });
        }
    }
}