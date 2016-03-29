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
    public class InMemoryCounterTests
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
                .RegisterType<InMemoryCounter>()
                .As<IDistributedCounter>(); // transient

            _container = builder.Build();
        }

        [TestMethod]
        public async Task Add_Counter_Test()
        {
            // arrange
            var requestId = Guid.NewGuid().ToString();
            var service = _container.Resolve<IDistributedCounter>();

            // act
            await service.Add(requestId);

            // assert
            Assert.AreEqual(1, await service.GetCurrent());
        }

        [TestMethod]
        public async Task Subtract_Non_Existing_Request_Test()
        {
            // arrange
            var requestId1 = Guid.NewGuid().ToString();
            var requestId2 = Guid.NewGuid().ToString();
            var service = _container.Resolve<IDistributedCounter>();

            // act
            await service.Add(requestId1);
            await service.Subtruct(requestId2);

            // assert
            Assert.AreEqual(1, await service.GetCurrent());
        }

        [TestMethod]
        public async Task Add_And_Subtruct_Test()
        {
            // arrange
            var requestId1 = Guid.NewGuid().ToString();
            var requestId2 = Guid.NewGuid().ToString();
            var service = _container.Resolve<IDistributedCounter>();

            // act
            await service.Add(requestId1);
            await service.Add(requestId2);
            await service.Subtruct(requestId1);

            // assert
            Assert.AreEqual(1, await service.GetCurrent());
        }

        [TestMethod]
        public async Task Add_With_Repeating_Request_Test()
        {
            // arrange
            var requestId = Guid.NewGuid().ToString();
            var service = _container.Resolve<IDistributedCounter>();

            // act
            await service.Add(requestId);
            await service.Add(requestId);

            // assert
            Assert.AreEqual(1, await service.GetCurrent());
        }

        [TestMethod]
        public async Task Add_And_Subtruct_With_Repeating_Request_Test()
        {
            // arrange
            var requestId1 = Guid.NewGuid().ToString();
            var requestId2 = Guid.NewGuid().ToString();
            var service = _container.Resolve<IDistributedCounter>();

            // act
            await service.Add(requestId1);
            await service.Add(requestId1);
            await service.Add(requestId2);
            await service.Add(requestId2);
            await service.Subtruct(requestId1);
            await service.Subtruct(requestId1);
            await service.Subtruct(requestId2);
            await service.Subtruct(requestId2);

            // assert
            Assert.AreEqual(0, await service.GetCurrent());
        }

        [TestMethod]
        public async Task Add_And_Subtruct_Multiple_Counters_Single_Subscription_Test()
        {
            // arrange
            var requestId = Guid.NewGuid().ToString();
            var service = _container.Resolve<IDistributedCounter>();
            List<int> result = new List<int>();

            // act
            service.OnCounterChangedAsync((counter) => {
                result.Add(counter);
                return Task.CompletedTask;
            });
            await service.Add(requestId);
            await service.Reset();

            // assert
            Assert.AreEqual(2, result.Count, "Invocation times is not as expected");
            Assert.AreEqual(1, result[0], "Value in first callback mismatch");
            Assert.AreEqual(0, result[1], "Value in second callback mismatch");

        }


        [TestCleanup]
        public void Cleanup()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}
