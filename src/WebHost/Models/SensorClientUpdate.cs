using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Models
{
    public class SensorClientUpdate : Sensor
    {
        public Guid ClientId { get; set; }

        public int NewState { get; set; }
    }
}