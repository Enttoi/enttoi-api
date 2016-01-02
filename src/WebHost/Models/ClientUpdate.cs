using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.Models
{
    public class ClientUpdate
    {
        public Guid clientId { get; set; }

        public bool newState { get; set; }
        
        public DateTime timestamp { get; set; }
    }
}