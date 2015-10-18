using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHost.Models;

namespace WebHost.Services
{
    public class AzureDocumentDb : IDocumentsService
    {
        private const string COLLECTION_CLIENTS = "clients";

        private readonly DocumentClient _client;
        private readonly string _dbName;
        private readonly Uri _clientCollectionLink;


        public AzureDocumentDb(string endPoint, string token, string dbName)
        {
            if (String.IsNullOrEmpty(endPoint)) throw new ArgumentNullException(nameof(endPoint));
            if (String.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));
            if (String.IsNullOrEmpty(dbName)) throw new ArgumentNullException(nameof(dbName));

            _client = new DocumentClient(new Uri(endPoint), token);
            _clientCollectionLink = new Uri($"dbs/{dbName}/colls/{COLLECTION_CLIENTS}");
        }

        public IList<Client> GetClients()
        {
            var _query = new SqlQuerySpec()
            {
                QueryText = "SELECT * FROM c WHERE c.isDisabled = false"
            };

            return _client
                .CreateDocumentQuery<Client>(_clientCollectionLink, _query)
                .ToList();
        }
    }
}