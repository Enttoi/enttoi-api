using System;

namespace Core.Models
{
    public class SensorStatePersisted : Sensor
    {
        public Guid ClientId { get; set; }

        public int State { get; set; }

        public DateTime StateUpdatedOn { get; set; }
    }
}