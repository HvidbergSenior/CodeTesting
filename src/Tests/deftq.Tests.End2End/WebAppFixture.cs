using System.Text.Json;
using System.Text.Json.Serialization;
using Baseline;
using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Serialization;
using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Infrastructure;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace deftq.Tests.End2End
{
    public class WebAppFixture : IAsyncLifetime
    {
        private readonly TestcontainerDatabase testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Username = "postgres",
            Password = "postgres",
        })
        .Build();

        internal Guid UserId { get; private set; }
        internal WebApplicationFactory<Program> AppFactory { get; private set; }
        internal HttpClient Client { get; private set; }
        
        public WebAppFixture()
        {
            AppFactory = new();
            Client = new HttpClient();
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }

        public async Task InitializeAsync()
        {
            await testcontainers.StartAsync();

            UserId = Guid.NewGuid(); 
            
#pragma warning disable CA2000
            AppFactory = new WebAppFactory<Program>().WithWebHostBuilder(builder => 
            {
                builder.ConfigureServices(services =>
                    { 
                        var config = new MartenConfig();
                        config.ShouldRecreateDatabase = true;
                        config.SchemaName = "public";
                        config.ConnectionString = testcontainers.ConnectionString;
                        services.AddMarten(config, true);
                        services.AddSingleton<IExecutionContext>(new FakeExecutionContext(UserId));
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => {});
                        services.Replace(ServiceDescriptor.Scoped<IBaseRateAndSupplementRepository>(_ => new BaseRateAndSupplementInMemoryRepository()));
                    }); 
                builder.ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole());
            });
#pragma warning restore CA2000
            
            Client = AppFactory.CreateClient();
            
            SystemTextJsonSerializerConfig.Options.Converters.AddRange(JsonSerializerOptions().Converters);
        }

        public Task DisposeAsync()
        {
            Client?.Dispose();
            AppFactory?.Dispose();
            return testcontainers.CleanUpAsync();
        }
        
#pragma warning disable CA1822
        internal JsonSerializerOptions JsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            jsonOptions.Converters.Add(new SystemTextDateOnlyJsonConverter());
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            jsonOptions.Converters.Add(new SystemTextDateTimeOffsetJsonConverter());
            return jsonOptions;
        }
#pragma warning restore CA1822
    }
}
