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
    public class CommonHub : Hub, IClientsNotifier
    {
        private readonly ILifetimeScope _hubLifetimeScope;
        private readonly ILogger _logger;
        private readonly ITableService _tableService;

        public CommonHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve dependencies from the hub lifetime scope
            _logger = _hubLifetimeScope.Resolve<ILogger>(); // singleton
            _tableService = _hubLifetimeScope.Resolve<ITableService>(); // singleton
        }

        public async Task RequestSensorsState(IList<Client> clients)
        {
            var states = await _tableService.GetSensorsStateAsync(clients);

            await Task.WhenAll(
                states.Select(state => this.SensorStateUpdate(state)));
        }

        public async Task SensorStateUpdate(Sensor sensor)
        {
            //Interlocked.
            await Clients.All.send();
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}