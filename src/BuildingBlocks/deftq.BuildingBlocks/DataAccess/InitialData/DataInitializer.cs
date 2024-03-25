using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Configuration;
using deftq.BuildingBlocks.DataAccess.Marten;
using Marten;
using Marten.Schema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace deftq.BuildingBlocks.DataAccess.InitialData
{
    public class DataInitializer : IInitialData
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMartenConfig _martenConfig;
        private readonly IEnvironment _env;
        private readonly ILogger<DataInitializer> _logger;

        public DataInitializer(IServiceProvider serviceProvider, IMartenConfig martenConfig, IEnvironment env, ILogger<DataInitializer> logger)
        {
            _serviceProvider = serviceProvider;
            _martenConfig = martenConfig;
            _env = env;
            _logger = logger;
        }

        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            _logger.LogInformation("Initializing data for environment {EnvironmentName}", _env.EnvironmentName);
            
            if (documentStore == null)
            {
                throw new ArgumentNullException(nameof(documentStore));
            }
            
            if (_martenConfig.ShouldRecreateDatabase && (_env.IsDevelopmentEnvironment() || _env.IsDemoEnvironment()))
            {
                _logger.LogInformation("Recreating database");
                await documentStore.Advanced.Clean.CompletelyRemoveAllAsync();
            }
                
            if (_martenConfig.PopulateWithDemoData)
            {
                var demoDataProviders = _serviceProvider.GetServices<IDemoDataProvider>();
                foreach (var dataProvider in demoDataProviders)
                {
                    await dataProvider.Populate(documentStore, cancellation);
                }
            }
        }
    }
}
