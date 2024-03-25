﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace deftq.BuildingBlocks.Integration.Scheduled
{
    internal sealed class CommandSchedulerBackgroundService : BackgroundService
    {
        private readonly ILogger<CommandSchedulerBackgroundService> logger;
        private readonly IServiceScopeFactory scopeFactory;

        public CommandSchedulerBackgroundService(ILogger<CommandSchedulerBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>")]
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var publisher = scope.ServiceProvider.GetRequiredService<ScheduledCommandPublisher>();
                    await publisher.PublishPendingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    // We don't want the background service to stop while the application continues,
                    // so catching and logging.
                    // Should certainly have some extra checks for the reasons, to act on it.
                    logger.LogError(ex, ex.Message, null!);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
