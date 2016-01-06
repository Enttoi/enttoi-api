using System;

namespace Core.Models
{
    /// <summary>
    /// Represents a sensor and its state that connected to the client
    /// </summary>
    public class SensorStatePersisted : Sensor
    {
        /// <summary>
        /// Client's unique identifier to which sensor is connected.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public Guid ClientId { get; set; }

        /// <summary>
        /// A current state of the sensor.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public int State { get; set; }

        /// <summary>
        /// Timestamp when the <see cref="State"/> field was last changed.
        /// </summary>
        public DateTime StateUpdatedOn { get; set; }
    }
}