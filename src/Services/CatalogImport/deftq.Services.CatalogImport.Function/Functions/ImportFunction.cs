using deftq.Services.CatalogImport.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace deftq.Services.CatalogImport.Function.Functions
{
    public class ImportFunction
    {
        public const string ImportTriggerQueueSetting = "ImportTriggerQueue";
        
        private readonly ICatalogImportService _importService;
        
        public ImportFunction(ICatalogImportService importService)
        {
            _importService = importService;
        }
        
        [FunctionName("TempoCatalogImportFunction")]
        public async Task RunAsync([QueueTrigger($"%{ImportTriggerQueueSetting}%")] string msg, ILogger log)
        {
            await _importService.ImportCatalog(CancellationToken.None);
        }
    }
}
