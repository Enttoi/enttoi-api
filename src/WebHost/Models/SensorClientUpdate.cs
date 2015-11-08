using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Models
{
    public class SensorClientUpdate : Sensor
    {
        public Guid clientId { get; set; }

        public int newState { get; set; }

        public DateTime timestamp { get; set; }

        public override string ToString() => $"{clientId}_{SensorType}_{SensorId}";
    }
}