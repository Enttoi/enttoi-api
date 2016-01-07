namespace Core.Models
{
    public class Sensor
    {
        /// <summary>
        /// Unique identifier within client of the sensor.
        /// </summary>
        /// <value>
        /// The sensor identifier.
        /// </value>
        public int sensorId { get; set; }

        /// <summary>
        /// The type of the sensor.
        /// </summary>
        /// <value>
        /// The type of the sensor.
        /// </value>
        public string sensorType { get; set; }
    }
}