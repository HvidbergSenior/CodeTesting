using deftq.BuildingBlocks.Integration.Outbox.Marten;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace deftq.BuildingBlocks.Integration.Outbox.Configuration
{
    public static class Config
    {
        public static IServiceCollection AddOutbox(this IServiceCollection services)
        {
            services.ConfigureMarten(opt => opt.Schema.Include(new OutboxRegistry()));
            services.TryAddScoped<IOutbox, MartenOutbox>();
            services.TryAddScoped<OutboxPublisher>();
            services.AddHostedService<OutboxPublisherBackgroundService>();
            return services;
        }
    }
}
