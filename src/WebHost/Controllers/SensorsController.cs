using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebHost.Controllers
{
    /// <summary>
    /// Sensors API
    /// </summary>
    public class SensorsController : ApiController
    {
        private readonly IDocumentsService _documentsService;
        private readonly ITableService _tableService;

        public SensorsController(IDocumentsService documentsService, ITableService tableService)
        {
            if (documentsService == null) throw new ArgumentNullException(nameof(documentsService));
            if (tableService == null) throw new ArgumentNullException(nameof(tableService));

            _documentsService = documentsService;
            _tableService = tableService;
        }

        /// <summary>
        /// Gets the sensors with their most recent state.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns>List of sensors</returns>
        [Route("sensors/{clientId:guid}")]
        [ResponseType(typeof(IList<SensorStatePersisted>))]
        public async Task<IHttpActionResult> GetSensors(Guid clientId)
        {
            var client = _documentsService.GetClient(clientId);

            if (client == null)
                return NotFound();

            return Ok(await _tableService.GetSensorsStateAsync(client));
        }
    }
}
