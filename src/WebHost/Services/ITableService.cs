using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHost.Models;

namespace WebHost.Services
{
    public interface ITableService
    {
        /// <summary>
        /// Gets the state of all sensors for specified clients.
        /// </summary>
        /// <param name="clients">The clients.</param>
        /// <returns></returns>
        Task<IList<SensorStatePersisted>> GetSensorsStateAsync(IList<Client> clients);
    }
}