using Newtonsoft.Json;
using System;

namespace Core.Models
{
    /// <summary>
    /// Represents a client to which multiple sensors are connected
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Client's globally unique identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        [JsonProperty(PropertyName = "id")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// Additional meta data which describes the client.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public string[] Tags { get; set; }

        /// <summary>
        /// A value indicating whether this client is online.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this client is online; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Timestamp when the <see cref="IsOnline"/> field was last changed.
        /// </summary>
        public DateTime IsOnlineChanged { get; set; }

        /// <summary>
        /// A list of connected sensors.
        /// </summary>
        /// <value>
        /// The sensors.
        /// </value>
        public Sensor[] Sensors { get; set; }
    }
}