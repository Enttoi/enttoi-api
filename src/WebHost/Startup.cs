using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebHost.Startup))]
namespace WebHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfigurations = new HttpConfiguration();

            // container
            var container = IocConfig.Register(app, httpConfigurations);

            // enable CORS for entire application
            app.UseCors(CorsOptions.AllowAll);

            // SignalR 
            var srConfig = new HubConfiguration
            {
                EnableJavaScriptProxies = true,
                EnableDetailedErrors = false,
                Resolver = new AutofacDependencyResolver(container)
            };
            app.MapSignalR("/signalr", srConfig);

            // enable swagger
            SwaggerConfig.Register(httpConfigurations);

            // enable web api and configure serialization of JSON
            httpConfigurations.MapHttpAttributeRoutes();
            httpConfigurations
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();
            app.UseWebApi(httpConfigurations);

            // configure notifications from service bus to hub
            SubscriptionsConfig.Register(container, srConfig.Resolver);
        }
    }
}
