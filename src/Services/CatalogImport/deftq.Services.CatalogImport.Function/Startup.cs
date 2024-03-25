using deftq.BuildingBlocks.Configuration;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.Catalog.Infrastructure.Configuration;
using deftq.Services.CatalogImport.Function;
using deftq.Services.CatalogImport.Function.Queue;
using deftq.Services.CatalogImport.Service.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace deftq.Services.CatalogImport.Function;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.UseBuildingBlocks();
        builder.Services.AddMarten(builder.GetContext().Configuration);
        
        builder.Services.AddHttpClient();
        builder.Services.AddCatalogImportService(builder.GetContext().Configuration);
        builder.Services.UseCatalog();
        
        builder.Services.AddSingleton<IQueueService>(new QueueService(builder.GetContext().Configuration));
    }
}
