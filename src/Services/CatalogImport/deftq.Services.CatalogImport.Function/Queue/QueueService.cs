using System.Text;
using Azure.Storage.Queues;
using deftq.Services.CatalogImport.Function.Functions;
using Microsoft.Extensions.Configuration;

namespace deftq.Services.CatalogImport.Function.Queue
{
    public interface IQueueService
    {
        Task ScheduleFetch(string message, CancellationToken cancellationToken);
        
        Task ScheduleImport(string message, CancellationToken cancellationToken);
    }
    
    public class QueueService : IQueueService
    {
        private string AzureWebJobsStorageSetting = "AzureWebJobsStorage";
        private readonly QueueClient _fetchTriggerQueueClient;
        private readonly QueueClient _importTriggerQueueClient;

        public QueueService(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>(AzureWebJobsStorageSetting);
            var fetchTriggerQueue = configuration.GetValue<string>(FetchFunction.FetchTriggerQueueSetting);
            var importTriggerQueue = configuration.GetValue<string>(ImportFunction.ImportTriggerQueueSetting);
            
            _fetchTriggerQueueClient = new QueueClient(connectionString, fetchTriggerQueue);
            _importTriggerQueueClient = new QueueClient(connectionString, importTriggerQueue);

            _fetchTriggerQueueClient.CreateIfNotExists();
            _importTriggerQueueClient.CreateIfNotExists();
        }

        public async Task ScheduleFetch(string message, CancellationToken cancellationToken)
        {
            var msg = EncodeMessage(message);
            await _fetchTriggerQueueClient.SendMessageAsync(msg, cancellationToken);
        }

        public async Task ScheduleImport(string message, CancellationToken cancellationToken)
        {
            var msg = EncodeMessage(message);
            await _importTriggerQueueClient.SendMessageAsync(msg, cancellationToken);
        }
        
        private string EncodeMessage(string message)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }
    }
}
