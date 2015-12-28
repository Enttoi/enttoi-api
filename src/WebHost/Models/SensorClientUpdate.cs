using Core.Models;
using System;

namespace WebHost.Models
{
    public class SensorClientUpdate : Sensor
    {
        public Guid clientId { get; set; }

        public int newState { get; set; }

        public DateTime timestamp { get; set; }

        public override string ToString() => $"{clientId}_{sensorType}_{sensorId}";
    }
}