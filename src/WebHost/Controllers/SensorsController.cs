using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebHost.Controllers
{
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

        [Route("sensors/{clientId:guid}")]
        public async Task<IHttpActionResult> GetSensors(Guid clientId)
        {
            var client = _documentsService.GetClient(clientId);

            if (client == null)
                return NotFound();

            return Ok(await _tableService.GetSensorsStateAsync(client));
        }
    }
}
