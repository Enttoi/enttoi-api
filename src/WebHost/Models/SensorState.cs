﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Models
{
    public class SensorState : Sensor
    {
        public Guid ClientId { get; set; }

        public int State { get; set; }

        public DateTime StateUpdatedOn { get; set; }
    }
}