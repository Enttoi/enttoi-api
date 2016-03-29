using Autofac;
using Core.Services;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHost.Models;

namespace WebHost.Hubs
{
    public class CommonHub : Hub
    {
        private readonly ILifetimeScope _hubLifetimeScope;
        private readonly ITableService _tableService;
        private readonly IDocumentsService _documentService;
        private readonly IDistributedCounter _counterService;

        public CommonHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve dependencies from the hub lifetime scope
            _documentService = _hubLifetimeScope.Resolve<IDocumentsService>(); // singleton
            _tableService = _hubLifetimeScope.Resolve<ITableService>(); // singleton
            _counterService = _hubLifetimeScope.Resolve<IDistributedCounter>(); // singleton
        }

        public override async Task OnConnected()
        {
            await requestInitialState();
        }

        public override async Task OnReconnected()
        {
            await requestInitialState();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            await _counterService.Subtruct(Context.ConnectionId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Requests the initial state. Calling this method will trigger to push current/initial state 
        /// to connected to hub users through the "channels" (hub events) which are used to send a regular updates.
        /// This way the hub users only subscribe to updates.
        /// </summary>
        /// <returns></returns>
        private async Task requestInitialState()
        {
            var onlineClients = _documentService.GetClients(true);
            var states = await _tableService.GetSensorsStateAsync(onlineClients);
            List<Task> tasks = new List<Task>();

            tasks.AddRange(states.Select(async state => await Clients.Client(Context.ConnectionId).sensorStatePush(new SensorClientUpdate
            {
                clientId = state.ClientId,
                sensorId = state.sensorId,
                sensorType = state.sensorType,
                newState = state.State,
                timestamp = state.StateUpdatedOn
            })));
            tasks.Add(_counterService.Add(Context.ConnectionId));

            await Task.WhenAll(tasks);
        }
    }
}
