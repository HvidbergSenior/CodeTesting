using deftq.BuildingBlocks.Integration.Inbox.Marten;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace deftq.BuildingBlocks.Integration.Inbox.Configuration
{
    public static class Config
    {
        public static IServiceCollection AddInbox(this IServiceCollection services)
        {
            services.ConfigureMarten(opt => opt.Schema.Include(new InboxRegistry()));
            services.TryAddScoped<IInbox, MartenInbox>();
            services.TryAddScoped<InboxPublisher>();
            services.AddHostedService<InboxPublisherBackgroundService>();
            return services;
        }
    }
}
