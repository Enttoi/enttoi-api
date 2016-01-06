using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebHost.Controllers
{
    /// <summary>
    /// Clients API
    /// </summary>
    public class ClientsController : ApiController
    {
        private readonly IDocumentsService _documentsService;

        public ClientsController(IDocumentsService documentsService)
        {
            if (documentsService == null) throw new ArgumentNullException(nameof(documentsService));
            
            _documentsService = documentsService;
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="onlineOnly">if set to <c>true</c> retrieves only client whos status is 'online'.</param>
        /// <returns>List of clients</returns>
        [Route("clients/all")]
        public IEnumerable<Client> GetClients(bool onlineOnly = false)
        {
            return _documentsService.GetClients(onlineOnly);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns>Single client</returns>
        [Route("clients/{clientId:guid}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClient(Guid clientId)
        {
            var client = _documentsService.GetClient(clientId);

            if (client == null)
                return NotFound();

            return Ok(client);
        }
    }
}
