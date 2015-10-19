using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Services
{
    public class StorageService : IStorageService
    {
        public StorageService(string storageAccount, string accessKey)
        {
            if (String.IsNullOrEmpty(storageAccount)) throw new ArgumentNullException(nameof(storageAccount));
            if (String.IsNullOrEmpty(accessKey)) throw new ArgumentNullException(nameof(accessKey));
            
        }


        public SensorState GetSensorState(Guid clientId, string sensorType, int sensorId)
        {
            if (Guid.Empty.Equals(clientId)) throw new ArgumentException(nameof(clientId));
            if (String.IsNullOrEmpty(sensorType)) throw new ArgumentNullException(nameof(sensorType));

        }
    }
}