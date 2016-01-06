using System.Web.Http;
using Swashbuckle.Application;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace WebHost
{
    internal static class SwaggerConfig
    {
        private static readonly IEnumerable<string> C_XML_DOCS = new[] { "WebHost.XML", "CORE.XML" };

        public static void Register(HttpConfiguration httpConfigurations)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            httpConfigurations
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Enttoi API");
                        c.IncludeXmlComments(combineXmlComments());
                    })
                .EnableSwaggerUi();
        }

        private static string combineXmlComments()
        {
            XElement xml = null;

            foreach (string fileName in C_XML_DOCS)
            {
                string fullPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\{fileName}";

                if (xml == null)
                    xml = XElement.Load(fullPath);
                else
                    XElement.Load(fullPath)
                        .Descendants()
                        .ToList()
                        .ForEach(e => xml.Add(e));
            }

            var finalFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\{String.Join("_", C_XML_DOCS)}.xml";

            if (xml != null)
                xml.Save(finalFile);

            return finalFile;
        }
    }
}
