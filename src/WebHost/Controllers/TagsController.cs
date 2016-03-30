using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebHost.Controllers
{
    /// <summary>
    /// Tags API
    /// </summary>
    public class TagsController : ApiController
    {
        private readonly IDocumentsService _documentsService;

        public TagsController(IDocumentsService documentsService)
        {
            if (documentsService == null) throw new ArgumentNullException(nameof(documentsService));
            _documentsService = documentsService;
        }

        
    }
}
