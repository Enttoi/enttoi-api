using System;

namespace Core.Models
{
    public class SensorStateMessage : Sensor
    {
        public Guid ClientId { get; set; }

        public int NewState { get; set; }

        public int PreviousState { get; set; }

        public long PreviousStateDurationMs { get; set; }

        public DateTime Timestamp { get; set; }
    }
}