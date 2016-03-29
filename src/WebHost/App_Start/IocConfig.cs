using Autofac;
using Autofac.Core;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Core;
using Core.Services;
using Owin;
using System;
using System.Reflection;
using System.Web.Http;
using WebHost.Logger;

namespace WebHost
{
    internal static class IocConfig
    {
        public static IContainer Register(IAppBuilder app, HttpConfiguration httpConfigurations)
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
                .RegisterType<InMemoryCounter>()
                .As<IDistributedCounter>()
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
    }
}