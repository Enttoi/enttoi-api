using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebHost.Logger;
using WebHost.Models;
using WebHost.Services;

namespace WebHost.Controllers
{
    public class ClientController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IDocumentsService _documentsService;

        public ClientController(ILogger logger, IDocumentsService documentsService)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (documentsService == null) throw new ArgumentNullException(nameof(documentsService));

            _logger = logger;
            _documentsService = documentsService;
        }

        [Route("client/all")]
        public IEnumerable<Client> GetClients()
        {
            return _documentsService.GetClients();
        }
    }
}
