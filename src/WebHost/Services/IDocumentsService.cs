using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHost.Models;

namespace WebHost.Services
{
    public interface IDocumentsService
    {
        IList<Client> GetClients();
    }
}