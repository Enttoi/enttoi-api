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
        private readonly ITableService _tableService;
        private readonly IDocumentsService _documentService;        

        public CommonHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve dependencies from the hub lifetime scope
            _documentService = _hubLifetimeScope.Resolve<IDocumentsService>(); // singleton
            _tableService = _hubLifetimeScope.Resolve<ITableService>(); // singleton
        }

        public async Task RequestInitialState()
        {
            var onlineClients = _documentService.GetClients(true);
            var states = await _tableService.GetSensorsStateAsync(onlineClients);

            await Task.WhenAll(
                states.Select(async state => await Clients.Client(Context.ConnectionId).sensorStatePush(new SensorClientUpdate
                {
                    clientId = state.ClientId,
                    sensorId = state.sensorId,
                    sensorType = state.sensorType,
                    newState = state.State,
                    timestamp = state.StateUpdatedOn
                })));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}
