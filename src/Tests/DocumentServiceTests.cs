using Autofac;
using Autofac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebHost.Logger;
using WebHost.Services;

namespace Tests
{
    [TestClass]
    public class DocumentServiceTests
    {
        IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();

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


            _container = builder.Build();
        }

        [TestMethod]
        public void GeClientsTest()
        {
            // arrange
            var service = _container.Resolve<IDocumentsService>();

            // act
            var result = service.GetClients();

            // assert
            Assert.IsNotNull(result, "Result was null");
            Assert.AreEqual(16, result.Count, "Number of clients is not 16");
        }


        [TestCleanup]
        public void Cleanup()
        {

        }
    }
}
