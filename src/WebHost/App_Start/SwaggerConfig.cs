using System.Web.Http;
using Swashbuckle.Application;
using System;

namespace WebHost
{
    internal static class SwaggerConfig
    {

        public static void Register(HttpConfiguration httpConfigurations)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            httpConfigurations
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Enttoi API");
                        c.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\Core.XML");
                        c.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\WebHost.XML");
                    })
                .EnableSwaggerUi();
        }
    }
}
