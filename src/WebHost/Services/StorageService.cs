using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHost.Models;

namespace WebHost.Services
{
    /// <summary>
    /// Thread safe type to perform operations against Azure's storage service.
    /// </summary>
    public class StorageService : IStorageService
    {
        private const string TABLE_SENSORS_STATE = "SensorsState";

        private readonly CloudStorageAccount _account;

        public StorageService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            _account = CloudStorageAccount.Parse(connectionString);
        }


        public async Task<IList<SensorState>> GetSensorsStateAsync(IList<Client> clients)
        {
            if (clients == null) throw new ArgumentNullException(nameof(clients));
            if (clients.Count == 0) throw new ArgumentException(nameof(clients));

            var operations = new TableBatchOperation();
            foreach (var client in clients)
            {
                foreach (var sensor in client.Sensors)
                {
                    operations.Add(TableOperation.Retrieve(client.ClientId.ToString(), $"{sensor.SensorType}_{sensor.SensorId}"));
                }
            }
            var tableRef = _account.CreateCloudTableClient().GetTableReference(TABLE_SENSORS_STATE);
            var result = await tableRef.ExecuteBatchAsync(operations);

            // TODO: continue
            throw new NotImplementedException();
        }
    }
}