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
    public class StorageServiceTests
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
                .RegisterType<StorageService>()
                .As<IStorageService>()
                .SingleInstance()
                .WithParameter("connectionString", "UseDevelopmentStorage=true");

            _container = builder.Build();
        }

        [TestMethod]
        public async Task GetStatusesTest()
        {
            // arrange
            var service = _container.Resolve<IStorageService>();
            var clients = new List<Client>() {
                new Client {
                    ClientId = Guid.Parse("e376cf38-3e4e-41da-839a-95a5a03d6d10"),
                    Sensors = new Sensor[] {
                        new Sensor {
                            SensorId = 1,
                            SensorType = "cabin_door"
                        },
                        new Sensor {
                            SensorId = 2,
                            SensorType = "cabin_door"
                        }}
                },
                new Client {
                    ClientId = Guid.Parse("887f6e3c-5f18-40a3-bb91-de911cdc77df"),
                    Sensors = new Sensor[] {
                        new Sensor {
                            SensorId = 1,
                            SensorType = "cabin_door"
                        },
                        new Sensor {
                            SensorId = 2,
                            SensorType = "cabin_door"
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
