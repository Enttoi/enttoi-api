using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHost.Logger;
using WebHost.Models;

namespace WebHost.Services
{
    /// <summary>
    /// Thread safe type to perform operations against Azure's storage service.
    /// </summary>
    public class StorageService : IStorageService
    {
        private const string TABLE_SENSORS_STATE = "SensorsState";
        private const int RETRY_COUNT = 3;
        private readonly TimeSpan RETRY_INTERVAL = TimeSpan.FromMilliseconds(500);

        private readonly CloudStorageAccount _account;

        public StorageService(string connectionString, ILogger logger)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            _account = CloudStorageAccount.Parse(connectionString);
        }


        public async Task<IList<SensorState>> GetSensorsStateAsync(IList<Client> clients)
        {
            if (clients == null) throw new ArgumentNullException(nameof(clients));
            if (clients.Count == 0) throw new ArgumentException(nameof(clients));

            var operationsTasks = new List<Task<TableResult>>();
            var tableRef = getTableReference(TABLE_SENSORS_STATE);

            foreach (var client in clients)
            {
                foreach (var sensor in client.Sensors)
                {
                    operationsTasks.Add(tableRef.ExecuteAsync(TableOperation.Retrieve(
                        client.ClientId.ToString(), $"{sensor.SensorType}_{sensor.SensorId}")));
                }
            }

            var result = await Task.WhenAll(operationsTasks);

            return result
                .Where(r => r.HttpStatusCode == 200)
                .Select(r => (DynamicTableEntity)r.Result)
                .Select(r => new SensorState
                {
                    ClientId = Guid.Parse(r.PartitionKey),
                    SensorId = r["SensorId"].Int32Value.Value,
                    SensorType = r["SensorType"].StringValue,
                    State = r["State"].Int32Value.Value,
                    StateUpdatedOn = r["TimeStamp"].DateTime.Value
                }).
                ToList();
        }


        private CloudTable getTableReference(string tableName)
        {
            var client = _account.CreateCloudTableClient();
            client.DefaultRequestOptions = new TableRequestOptions()
            {
                RetryPolicy = new LinearRetry(RETRY_INTERVAL, RETRY_COUNT),
                LocationMode = LocationMode.PrimaryThenSecondary
            };

            return client.GetTableReference(tableName);
        }
    }
}