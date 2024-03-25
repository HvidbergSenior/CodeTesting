using deftq.BuildingBlocks.Integration.Scheduled.Marten;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace deftq.BuildingBlocks.Integration.Scheduled.Configuration
{
    public static class Config
    {
        public static IServiceCollection AddCommandScheduler(this IServiceCollection services)
        {
            services.ConfigureMarten(opt => opt.Schema.Include(new CommandSchedulerRegistry()));
            services.TryAddScoped<ICommandScheduler, CommmandScheduler>();
            services.TryAddScoped<ICommandSchedulerRepository, MartenCommandSchedulerRepository>();
            services.TryAddScoped<ScheduledCommandPublisher>();
            services.AddHostedService<CommandSchedulerBackgroundService>();

            return services;
        }
    }
}
