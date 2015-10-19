using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHost.Models;

namespace WebHost.Services
{
    public interface IStorageService
    {
        SensorState GetSensorState(Guid clientId, string sensorType, int sensorId);
    }
}