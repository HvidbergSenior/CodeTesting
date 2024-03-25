using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Configuration;
using deftq.Catalog.Application;
using deftq.Services.CatalogImport.Service.CatalogApi;
using deftq.UserAccess.Application.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace deftq.Services.CatalogImport.Service.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddCatalogImportService(this IServiceCollection services, IConfiguration config)
        {
            var catalogApiConfig = config.GetSection(nameof(CatalogApiConfig)).Get<CatalogApiConfig>();
            services.AddSingleton<ICatalogApiConfig>(catalogApiConfig);
            
            services.AddScoped<IIdentityResolver, IdentityResolver>();
            services.AddScoped<ICatalogApiClient, CatalogApiClient>();
            services.AddScoped<ICatalogFetchService, CatalogFetchService>();
            services.AddScoped<ICatalogImportService, CatalogImportService>();
            services.AddMediatorHandlersFromAssembly(typeof(ApplicationTarget).Assembly);
            return services;
        }
        
        public class CatalogApiConfig : ICatalogApiConfig
        {
            private const string DefaultEVUTempoKey = "secret";
            private const string DefaultEVUTempoKeyHeader = "Ocp-Apim-Subscription-Key";
            private const string DefaultEVUTempoMaterialEndpointUrl = "https://api.evu.dk/tempo/materials";
            private const string DefaultEVUTempoOperationEndpointUrl = "https://api.evu.dk/tempo/operations";
            
            public string EVUTempoKey { get; set; } = DefaultEVUTempoKey;

            public string EVUTempoKeyHeader { get; set; } = DefaultEVUTempoKeyHeader;

            public string EVUTempoMaterialEndpointUrl { get; set; } = DefaultEVUTempoMaterialEndpointUrl;

            public string EVUTempoOperationEndpointUrl { get; set; } = DefaultEVUTempoOperationEndpointUrl;
        }
    }
}
