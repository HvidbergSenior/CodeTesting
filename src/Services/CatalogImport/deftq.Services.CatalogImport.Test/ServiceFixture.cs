using System.Globalization;
using deftq.BuildingBlocks.Configuration;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Integration.Inbox.Configuration;
using deftq.Catalog.Infrastructure.Configuration;
using deftq.Services.CatalogImport.Service.CatalogApi;
using deftq.Services.CatalogImport.Service.Configuration;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Environment = System.Environment;

namespace deftq.Services.CatalogImport.Test
{
    public class ServiceFixture : IAsyncLifetime
    {
        private readonly TestcontainerDatabase testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration { Database = "db", Username = "postgres", Password = "postgres", })
            .Build();

        private IHost? _host;
        internal IHost Host => _host!;

        public ServiceFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }

        public async Task InitializeAsync()
        {
            await testcontainers.StartAsync();

            HostApplicationBuilder builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(Array.Empty<string>());
            builder.Services.UseBuildingBlocks();
            var config = new MartenConfig();
            config.ShouldRecreateDatabase = true;
            config.SchemaName = "public";
            config.ConnectionString = testcontainers.ConnectionString;
            builder.Services.AddMarten(config, true);
            builder.Services.AddInbox();

            builder.Services.AddCatalogImportService(builder.Configuration);
            builder.Services.UseCatalog();

            builder.Services.Replace(ServiceDescriptor.Scoped<ICatalogApiClient>(_ =>
            {
                var fakeCatalogApiClient = CreateFakeApiClient();
                return fakeCatalogApiClient;
            }));

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();

            _host = builder.Build();
            await _host.StartAsync();
        }

        private ICatalogApiClient CreateFakeApiClient()
        {
            IList<MaterialResponse> materialResponses = Enumerable.Range(1, End2EndTest.MaterialCount).Select(i =>
                new MaterialResponse(string.Format(new NumberFormatInfo(), "{0:D13}", i), "Material " + i, "meter", new List<MountingResponse>())).ToList();
            return new FakeCatalogApiClient(materialResponses);
        }

        public Task DisposeAsync()
        {
            _host?.Dispose();
            return testcontainers.CleanUpAsync();
        }
    }
}
