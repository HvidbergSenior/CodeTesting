using deftq.Services.CatalogImport.Function.Queue;
using deftq.Services.CatalogImport.Service;
using deftq.Services.CatalogImport.Service.CatalogApi;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace deftq.Services.CatalogImport.Function.Functions
{
    public class FetchFunction
    {
        public const string FetchTriggerQueueSetting = "FetchTriggerQueue";
        
        private readonly ICatalogFetchService _fetchService;
        private readonly IQueueService _queueService;
        
        public FetchFunction(ICatalogFetchService fetchService, IQueueService queueService)
        {
            _fetchService = fetchService;
            _queueService = queueService;
        }
        
        [FunctionName("TempoApiFetchFunction")]
        public async Task RunAsync([QueueTrigger($"%{FetchTriggerQueueSetting}%")] string msg, ILogger log)
        {
            var fetchedPage = MaterialCatalogResponse.Empty();
            var cancellationToken = CancellationToken.None;
            try
            {
                fetchedPage = await _fetchService.FetchCatalog(cancellationToken);
            }
            catch (Exception e)
            {
                log.LogError(e, "Error fetching page");
                throw;
            }
            finally
            {
                if (fetchedPage.HasNextPage)
                {
                    await _queueService.ScheduleFetch($"Schedule fetch of next catalog page from {fetchedPage.NextPageUrl}",cancellationToken);
                }
                else
                {
                    await _queueService.ScheduleImport("Schedule import of catalog", cancellationToken);
                }
            }
        }
    }
}
