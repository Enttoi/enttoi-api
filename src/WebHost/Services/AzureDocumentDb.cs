using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Client.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using WebHost.Logger;
using WebHost.Models;

namespace WebHost.Services
{
    /// <summary>
    /// Thread safe type to perform operations against Azure's DocumentDB service.
    /// </summary>
    public class AzureDocumentDb : IDocumentsService, IDisposable
    {
        private const string COLLECTION_CLIENTS = "clients";
        private const int RETRY_COUNT = 3;
        private readonly TimeSpan RETRY_INTERVAL = TimeSpan.FromMilliseconds(500);

        // from here: https://msdn.microsoft.com/library/azure/microsoft.azure.documents.client.documentclient.aspx
        // this type is thread safe
        private readonly IReliableReadWriteDocumentClient _client;
        private readonly Uri _clientCollectionLink;


        public AzureDocumentDb(string endPoint, string accessKey, string dbName, ILogger logger)
        {
            if (String.IsNullOrEmpty(endPoint)) throw new ArgumentNullException(nameof(endPoint));
            if (String.IsNullOrEmpty(accessKey)) throw new ArgumentNullException(nameof(accessKey));
            if (String.IsNullOrEmpty(dbName)) throw new ArgumentNullException(nameof(dbName));

            _client = new DocumentClient(new Uri(endPoint), accessKey)
                .AsReliable(new FixedInterval(RETRY_COUNT, RETRY_INTERVAL));
            
            // TODO: to warm-up it worth calling 
            // await _client.OpenAsync()
            _clientCollectionLink = new Uri($"dbs/{dbName}/colls/{COLLECTION_CLIENTS}", UriKind.Relative);
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

        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();
        }
    }
}