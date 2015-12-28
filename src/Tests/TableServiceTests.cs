using Autofac;
using Core;
using Core.Models;
using Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebHost.Logger;

namespace Tests
{
    [TestClass]
    public class TableServiceTests
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
                .RegisterType<AzureTableStorageService>()
                .As<ITableService>()
                .SingleInstance()
                .WithParameter("connectionString", "UseDevelopmentStorage=true");

            _container = builder.Build();
        }

        [TestMethod]
        public async Task GetStatusesTest()
        {
            // arrange
            var service = _container.Resolve<ITableService>();
            var clients = new List<Client>() {
                new Client {
                    ClientId = Guid.Parse("e376cf38-3e4e-41da-839a-95a5a03d6d10"),
                    Sensors = new Sensor[] {
                        new Sensor {
                            sensorId = 1,
                            sensorType = "cabin_door"
                        },
                        new Sensor {
                            sensorId = 2,
                            sensorType = "cabin_door"
                        }}
                },
                new Client {
                    ClientId = Guid.Parse("887f6e3c-5f18-40a3-bb91-de911cdc77df"),
                    Sensors = new Sensor[] {
                        new Sensor {
                            sensorId = 1,
                            sensorType = "cabin_door"
                        },
                        new Sensor {
                            sensorId = 2,
                            sensorType = "cabin_door"
                        }}
                }
            };

            // act
            var statuses = await service.GetSensorsStateAsync(clients);

            // assert
            Assert.IsNotNull(statuses, "Result was null");
            Assert.AreEqual(4, statuses.Count, "Number of sensors is not 4");
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}
