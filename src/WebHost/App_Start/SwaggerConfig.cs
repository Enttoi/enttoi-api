using System.Web.Http;
using Swashbuckle.Application;
using System;
using System.IO;

namespace WebHost
{
    internal static class SwaggerConfig
    {

        public static void Register(HttpConfiguration httpConfigurations)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            var coreXml = new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\Core.XML");
            var hostXml = new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\WebHost.XML");

            httpConfigurations
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Enttoi API");
                        if (coreXml.Exists) c.IncludeXmlComments(coreXml.FullName);
                        if (hostXml.Exists) c.IncludeXmlComments(hostXml.FullName);
                    })
                .EnableSwaggerUi();
        }
    }
}
