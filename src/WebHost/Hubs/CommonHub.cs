using Autofac;
using Core.Services;
using Microsoft.AspNet.SignalR;
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

        public CommonHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve dependencies from the hub lifetime scope
            _documentService = _hubLifetimeScope.Resolve<IDocumentsService>(); // singleton
            _tableService = _hubLifetimeScope.Resolve<ITableService>(); // singleton
        }

        /// <summary>
        /// Requests the initial state. Calling this method will trigger to push current/initial state 
        /// to connected to hub users through the "channels" (hub events) which are used to send a regular updates.
        /// This way the hub users only subscribe to updates.
        /// </summary>
        /// <returns></returns>
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

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}
