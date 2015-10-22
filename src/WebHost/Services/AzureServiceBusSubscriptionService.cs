using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHost.Logger;

namespace WebHost.Services
{
    public class AzureServiceBusSubscriptionService : ISubscriptionService, IDisposable
    {
        private const string TOPIC_SENSORS_STATE = "sensor-state-changed";
        private const string SUBSCRIPTION_NAME = "AllMessages";

        private const int RETRY_COUNT = 3;
        private readonly TimeSpan RETRY_INTERVAL = TimeSpan.FromMilliseconds(500);

        SubscriptionClient _client;
        private readonly ILogger _logger;

        public AzureServiceBusSubscriptionService(string connectionString, ILogger logger)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.SubscriptionExists(TOPIC_SENSORS_STATE, SUBSCRIPTION_NAME))
                namespaceManager.CreateSubscription(TOPIC_SENSORS_STATE, SUBSCRIPTION_NAME);

            _client = SubscriptionClient.CreateFromConnectionString(connectionString, 
                TOPIC_SENSORS_STATE, SUBSCRIPTION_NAME, ReceiveMode.ReceiveAndDelete);            
        }

        public void OnMessages(Action<string> callback)
        {
            var options = new OnMessageOptions()
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };

            _client.OnMessage((message) => {
                callback(message.GetBody<string>());                
            }, options);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_client == null || _client.IsClosed)
                return;

            _client.Close();

            // Suppress finalization of this disposed instance
            if (disposing)
                GC.SuppressFinalize(this);
        }

        ~AzureServiceBusSubscriptionService()
        {
            this.Dispose(false);
        }
    }
}