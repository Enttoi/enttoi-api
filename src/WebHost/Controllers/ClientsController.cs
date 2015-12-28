using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace WebHost.Controllers
{
    public class ClientsController : ApiController
    {
        private readonly IDocumentsService _documentsService;

        public ClientsController(IDocumentsService documentsService)
        {
            if (documentsService == null) throw new ArgumentNullException(nameof(documentsService));
            
            _documentsService = documentsService;
        }

        [Route("clients/all")]
        public IEnumerable<Client> GetClients(bool onlineOnly = false)
        {
            return _documentsService.GetClients(onlineOnly);
        }

        [Route("clients/{clientId:guid}")]
        public Client GetClient(Guid clientId)
        {
            return _documentsService.GetClient(clientId);
        }
    }
}
