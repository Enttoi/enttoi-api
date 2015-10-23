using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHost.Models;

namespace WebHost.Hubs
{
    public interface IClientsNotifier
    {
        Task SensorStateUpdate(Sensor sensor);
    }
}