using Autofac;
using Autofac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebHost.Logger;
using WebHost.Models;
using WebHost.Services;

namespace Tests
{
    [TestClass]
    public class SubscriptionServiceTests
    {
        IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            // application related
            builder
                .RegisterType<TraceLogger>()
                .As<ILogger>()
                .SingleInstance();

            builder
                .RegisterType<AzureServiceBusSubscriptionService>()
                .As<ISubscriptionService>()
                .SingleInstance()
                .WithParameter("connectionString", Environment.GetEnvironmentVariable("SERVICEBUS_CONNECTION_STRING"));

            _container = builder.Build();
        }

        [TestMethod]
        public async Task OnSensorStateChangedTest()
        {
            // arrange
            var service = _container.Resolve<ISubscriptionService>();

            // act
            var messages = new List<SensorStateChanges>();
            service.OnSensorStateChanged((message) =>
            {
                messages.Add(message);
            });

            await Task.Delay(10000000);

            // assert
            // meantime at least not having exceptions 
        }

        [TestMethod]
        public async Task OnSensorStateChangedAsyncTest()
        {
            // arrange
            var service = _container.Resolve<ISubscriptionService>();

            // act
            var messages = new List<SensorStateChanges>();
            service.OnSensorStateChangedAsync(async (message) => {
                await Task.CompletedTask;
                messages.Add(message);
            });

            await Task.Delay(10000000);

            // assert
            // meantime at least not having exceptions 
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}
