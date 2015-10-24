using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Models
{
    public class SensorStateMessage : Sensor
    {
        public Guid ClientId { get; set; }

        public int NewState { get; set; }

        public int PreviousState { get; set; }

        public long PreviousStateDurationMs { get; set; }

        public DateTime Timestamp { get; set; }

        public override string ToString() => $"{ClientId}_{SensorType}_{SensorId}";
    }
}