using Autofac;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHost.Logger;
using WebHost.Services;

namespace WebHost.Hubs
{
    public class APIHub : Hub
    {
        private readonly ILifetimeScope _hubLifetimeScope;
        private readonly ILogger _logger;
        private readonly IDocumentsService _documents;

        public APIHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve dependencies from the hub lifetime scope
            _logger = _hubLifetimeScope.Resolve<ILogger>();
            _documents = _hubLifetimeScope.Resolve<IDocumentsService>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}