using deftq.Services.CatalogImport.Function.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace deftq.Services.CatalogImport.Function.Functions
{
    public class MasterTimerFunction
    {
        private readonly IQueueService _queueService;

        public MasterTimerFunction(IQueueService queueService)
        {
            _queueService = queueService;
        }
        
        [FunctionName("TempoCatalogTimerFunction")]
        public async Task RunAsync([TimerTrigger("%TempoCatalogTimerCron%", RunOnStartup = false)] TimerInfo myTimer, ILogger log)
        {
            await _queueService.ScheduleFetch("Schedule check for new published catalog", CancellationToken.None);
        }
    }
}
