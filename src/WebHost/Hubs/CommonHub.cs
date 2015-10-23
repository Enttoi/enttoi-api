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

namespace WebHost.Hubs
{
    public class CommonHub : Hub, IClientsNotifier
    {
        private readonly ILifetimeScope _hubLifetimeScope;
        private readonly ILogger _logger;

        public CommonHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve dependencies from the hub lifetime scope
            _logger = _hubLifetimeScope.Resolve<ILogger>();
        }

        public Task SensorStateUpdate(Sensor sensor)
        {
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