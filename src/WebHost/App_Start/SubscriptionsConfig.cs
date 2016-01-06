using Autofac;
using Core.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using WebHost.Hubs;
using WebHost.Models;

namespace WebHost
{
    internal static class SubscriptionsConfig
    {
        public static void Register(IContainer container, IDependencyResolver resolver)
        {
            var service = container.Resolve<ISubscriptionService>();
            var hub = resolver.Resolve<IConnectionManager>().GetHubContext<CommonHub>();

            service.OnSensorStateChangedAsync(async (state) =>
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

            service.OnClientStateChangedAsync(async (state) =>
            {
                await hub.Clients.All.clientStatePush(new ClientUpdate
                {
                    clientId = state.ClientId,
                    newState = state.NewState,
                    timestamp = state.Timestamp
                });
            });
        }
    }
}