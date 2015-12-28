using Core.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class AzureServiceBusSubscriptionService : ISubscriptionService, IDisposable
    {
        private const string TOPIC_SENSORS_STATE = "sensor-state-changed";
        private const string SUBSCRIPTION_NAME = "AllMessages";

        private const int RETRY_COUNT = 3;
        private readonly TimeSpan RETRY_INTERVAL = TimeSpan.FromMilliseconds(500);

        private SubscriptionClient _client;
        private readonly OnMessageOptions _messageOptions;
        private readonly ILogger _logger;

        public AzureServiceBusSubscriptionService(string connectionString, ILogger logger)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.SubscriptionExists(TOPIC_SENSORS_STATE, SUBSCRIPTION_NAME))
                namespaceManager.CreateSubscription(TOPIC_SENSORS_STATE, SUBSCRIPTION_NAME);

            _messageOptions = new OnMessageOptions()
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };

            _client = SubscriptionClient.CreateFromConnectionString(connectionString,
                TOPIC_SENSORS_STATE, SUBSCRIPTION_NAME, ReceiveMode.ReceiveAndDelete);
        }

        public void OnSensorStateChanged(Action<SensorStateMessage> callback)
        {
            _client.OnMessage((message) =>
            {
                using (var stream = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8))
                {
                    callback(JsonConvert.DeserializeObject<SensorStateMessage>(stream.ReadToEnd()));
                }
            }, _messageOptions);
        }

        public void OnSensorStateChangedAsync(Func<SensorStateMessage, Task> callback)
        {
            _client.OnMessageAsync(async (message) =>
            {
                using (var stream = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8))
                {
                    await callback(JsonConvert.DeserializeObject<SensorStateMessage>(await stream.ReadToEndAsync()));
                }
            }, _messageOptions);
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