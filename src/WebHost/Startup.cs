using Autofac;
using Autofac.Core;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Reflection;
using System.Web.Http;
using WebHost.Hubs;
using WebHost.Logger;
using WebHost.Models;
using WebHost.Services;

[assembly: OwinStartup(typeof(WebHost.Startup))]
namespace WebHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfigurations = new HttpConfiguration();

            // container
            var container = configureIoC(app, httpConfigurations);

            // SignalR routes
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(new HubConfiguration
                {
                    EnableJavaScriptProxies = true,
                    EnableDetailedErrors = false,
                    Resolver = new AutofacDependencyResolver(container)
                });
            });

            // enable web api and configure serialization of JSON
            httpConfigurations.MapHttpAttributeRoutes();
            httpConfigurations
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();
            app.UseWebApi(httpConfigurations);

            // configure notifications from service bus to hub
            configureSubscriptions(container);
        }

        private IContainer configureIoC(IAppBuilder app, HttpConfiguration httpConfigurations)
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            // web api controllers
            builder.RegisterApiControllers(assembly);

            // SignalR all hubs
            builder.RegisterHubs(assembly);

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

            builder
                .RegisterType<AzureServiceBusSubscriptionService>()
                .As<ISubscriptionService>()
                .SingleInstance()
                .WithParameter("connectionString", Environment.GetEnvironmentVariable("SERVICEBUS_CONNECTION_STRING"));
            
            var container = builder.Build();

            // resolving in OWIN middleware 
            app.UseAutofacMiddleware(container);

            // web api registration
            httpConfigurations.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }

        private void configureSubscriptions(IContainer container)
        {
            var service = container.Resolve<ISubscriptionService>();
            var publisher = container.Resolve<CommonHub>();

            service.OnSensorStateChangedAsync(async (state) =>
            {
                await publisher.Clients.All.SensorStatePush(new SensorClientUpdate
                {
                    ClientId = state.ClientId,
                    SensorId = state.SensorId,
                    SensorType = state.SensorType,
                    NewState = state.NewState,
                    Timestamp = state.Timestamp
                });
            });
        }
    }
}
