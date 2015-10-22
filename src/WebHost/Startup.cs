using Autofac;
using Autofac.Core;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebHost.Logger;
using WebHost.Services;

[assembly: OwinStartup(typeof(WebHost.Startup))]
namespace WebHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // container
            var container = configureIoC(app);

            // SignalR routes
            app.MapSignalR("/signalr", new HubConfiguration
            {
                EnableJavaScriptProxies = false,
                Resolver = new AutofacDependencyResolver(container)
            });
        }

        private IContainer configureIoC(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            // SignalR all hubs
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            // application related
            builder
                .RegisterType<TraceLogger>()
                .As<ILogger>()
                .SingleInstance();

            builder
                .RegisterType<AzureDocumentDb>()
                .As<IDocumentsService>()
                .SingleInstance()
                .WithParameters(new Parameter[] {
                    new PositionalParameter(0 , Environment.GetEnvironmentVariable("DOCUMENT_DB_ENDPOINT")),
                    new PositionalParameter(1, Environment.GetEnvironmentVariable("DOCUMENT_DB_ACCESS_KEY")),
                    new PositionalParameter(2, Environment.GetEnvironmentVariable("DOCUMENT_DB_NAME") ?? "development")});

            builder
                .RegisterType<AzureTableStorageService>()
                .As<ITableService>()
                .SingleInstance()
                .WithParameter("connectionString", Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING") ?? "UseDevelopmentStorage=true");

            var container = builder.Build();

            // OWIN related registrations
            app.UseAutofacMiddleware(container);

            return container;
        }
    }
}