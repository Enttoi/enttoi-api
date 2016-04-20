using Core;
using Core.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebHost.Controllers
{
    /// <summary>
    /// Health monitoring API
    /// </summary>
    public class HealthController : ApiController
    {
        private readonly IDocumentsService _documentsService;
        private readonly ITableService _tableService;
        private readonly ILogger _loger;

        public HealthController(IDocumentsService documentsService, ITableService tableService, ILogger loger)
        {
            if (documentsService == null) throw new ArgumentNullException(nameof(documentsService));
            if (tableService == null) throw new ArgumentNullException(nameof(tableService));
            if (loger == null) throw new ArgumentNullException(nameof(loger));

            _documentsService = documentsService;
            _tableService = tableService;
            _loger = loger;
        }

        /// <summary>
        /// Gets overall indication of API health
        /// </summary>
        /// <response code="200">API is in good state</response>
        /// <response code="404">Some or all of the parts of API is not functional</response>
        [Route("health/status")]
        public async Task<IHttpActionResult> GetStatus()
        {
            try
            {
                var client = _documentsService.GetClients()[0];
                await _tableService.GetSensorsStateAsync(client);
                return Ok(new
                {
                    Status = "OK"
                });
            }
            catch (Exception ex)
            {
                _loger.Log("Error occurred in health API", ex.Message);
                return BadRequest("Error occurred in health API");
            }
        }
    }
}
