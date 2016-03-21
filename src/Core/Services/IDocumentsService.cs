using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.Services
{
    public interface IDocumentsService
    {
        IList<Client> GetClients(bool onlineOnly = false);

        Client GetClient(Guid clientId);

        StatsSensorStates GetHourlyStats(Guid clientId, int sensorId, DateTime from, DateTime to);
    }
}