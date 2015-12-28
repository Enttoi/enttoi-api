using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
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