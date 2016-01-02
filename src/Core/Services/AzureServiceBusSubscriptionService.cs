using Core.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class AzureServiceBusSubscriptionService : ISubscriptionService, IDisposable
    {
        private readonly string[] TOPICS = new[] { "sensor-state-changed", "client-state-changed" };
        private const string SUBSCRIPTION_PREFIX = "AllMessages";

        private const int RETRY_COUNT = 3;
        private readonly TimeSpan RETRY_INTERVAL = TimeSpan.FromMilliseconds(500);

        private readonly Dictionary<string, SubscriptionClient> _clients;
        private readonly OnMessageOptions _messageOptions;
        private readonly ILogger _logger;

        public AzureServiceBusSubscriptionService(string connectionString, ILogger logger)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            _clients = new Dictionary<string, SubscriptionClient>();
            foreach (var topic in TOPICS)
            {
                if (!namespaceManager.SubscriptionExists(topic, $"{SUBSCRIPTION_PREFIX}_{topic}"))
                    namespaceManager.CreateSubscription(topic, $"{SUBSCRIPTION_PREFIX}_{topic}");

                _clients.Add(topic, SubscriptionClient.CreateFromConnectionString(
                    connectionString, 
                    topic, 
                    $"{SUBSCRIPTION_PREFIX}_{topic}", 
                    ReceiveMode.ReceiveAndDelete));
            }

            _messageOptions = new OnMessageOptions()
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };
        }

        public void OnSensorStateChangedAsync(Func<SensorStateMessage, Task> callback)
        {
            _clients[TOPICS[0]].OnMessageAsync(async (message) =>
            {
                using (var stream = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8))
                {
                    var _payload = await stream.ReadToEndAsync();
                    _logger.Log($"Received sensor state {_payload}");
                    await callback(JsonConvert.DeserializeObject<SensorStateMessage>(_payload));
                }
            }, _messageOptions);
        }

        public void OnClientStateChangedAsync(Func<ClientStateMessage, Task> callback)
        {
            _clients[TOPICS[1]].OnMessageAsync(async (message) =>
            {
                using (var stream = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8))
                {
                    var _payload = await stream.ReadToEndAsync();
                    _logger.Log($"Received client state {_payload}");
                    await callback(JsonConvert.DeserializeObject<ClientStateMessage>(_payload));
                }
            }, _messageOptions);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_clients == null || _clients.Count == 0)
                return;

            foreach (var client in _clients)
            {
                if(!client.Value.IsClosed)
                    client.Value.Close();
            }

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