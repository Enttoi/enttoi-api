using Autofac;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHost.Logger;
using WebHost.Services;
using WebHost.Models;
using System.Threading;

namespace WebHost.Hubs
{
    public class CommonHub : Hub
    {
        private readonly ILifetimeScope _hubLifetimeScope;
        private readonly ILogger _logger;
        private readonly ITableService _tableService;
        private readonly IDocumentsService _documentService;

        public CommonHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve dependencies from the hub lifetime scope
            _logger = _hubLifetimeScope.Resolve<ILogger>(); // singleton
            _tableService = _hubLifetimeScope.Resolve<ITableService>(); // singleton
            _tableService = _hubLifetimeScope.Resolve<ITableService>(); // singleton
        }

        public async Task RequestSensorsState()
        {
            var onlineClients = _documentService.GetClients(true);
            var states = await _tableService.GetSensorsStateAsync(onlineClients);

            await Task.WhenAll(
                states.Select(state => this.SensorStateUpdate(state)));
        }

        // TODO: move the method outside of HUB
        internal async Task SensorStateUpdate(Sensor sensor)
        {
            if (sensor is SensorStateMessage)
            {
                var sensorMessage = sensor as SensorStateMessage;

                // update sent from Gateway => all clients needs to be notified

                // resolution of ~20ms
                // TODO: Interlocked.Exchange(ref SOME_VARIABLE_TOSTORE_LATEST_TICK, sensorMessage.Timestamp.Ticks);

                await Clients.All.SensorStateUpdate(new SensorClientUpdate
                {
                    ClientId = sensorMessage.ClientId,
                    SensorId = sensorMessage.SensorId,
                    SensorType = sensorMessage.SensorType,
                    NewState = sensorMessage.NewState
                });
            }
            else if (sensor is SensorStatePersisted)
            {
                // specific client requested current state
                var sensorState = sensor as SensorStatePersisted;

                // check that no newer notifications were sent
                // TODO: if(Interlocked.CompareExchange(ref SOME_VARIABLE_TOSTORE_LATEST_TICK, sensorState.StateUpdatedOn.Ticks) != sensorState.StateUpdatedOn.Ticks)
                //{
                // skip sending state as it was already send by update
                //}
                //else
                {
                    await Clients.Client(Context.ConnectionId).SensorStateUpdate(new SensorClientUpdate
                    {
                        ClientId = sensorState.ClientId,
                        SensorId = sensorState.SensorId,
                        SensorType = sensorState.SensorType,
                        NewState = sensorState.State
                    });
                }
            }
            else
                throw new InvalidOperationException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}