using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebHost.Controllers
{
    /// <summary>
    /// Statistics API
    /// </summary>
    public class StatsController : ApiController
    {
        private readonly IDocumentsService _documentsService;

        public StatsController(IDocumentsService documentsService)
        {
            if (documentsService == null) throw new ArgumentNullException(nameof(documentsService));

            _documentsService = documentsService;
        }

        /// <summary>
        /// Gets the sensor's state statistics.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="sensorId">The sensor identifier.</param>
        /// <param name="from">Returns statistics from date.</param>
        /// <param name="to">Returns statistics until date.</param>
        /// <returns>States and their time sensor remained in</returns>
        /// <response code="404">Client or sensor was not found for provided IDs</response>
        [Route("stats/sensor-state/{clientId:guid}/{sensorId:int}")]
        [ResponseType(typeof(StatsSensorStates))]
        public IHttpActionResult GetSensors(Guid clientId, int sensorId, DateTime from, DateTime to)
        {
            var client = _documentsService.GetClient(clientId);

            if (client == null || !client.Sensors.Any(s => s.sensorId == sensorId))
                return NotFound();

            var ran = new Random();
            return Ok(new StatsSensorStates() {
                { -1, ran.Next(100) + 1 },
                { 0, ran.Next(100) + 1 },
                { 1, ran.Next(100) + 1 }
            });
        }
    }
}
