using System.Linq.Expressions;
using Marten;

namespace deftq.BuildingBlocks.Integration.Scheduled.Configuration
{
    internal sealed class CommandSchedulerRegistry : MartenRegistry
    {
        public CommandSchedulerRegistry()
        {
            const string schemaname = "Integration";

            For<ScheduledCommand>().DatabaseSchemaName(schemaname);
            For<ScheduledCommand>().UseOptimisticConcurrency(enabled: true);
            var columns = new Expression<Func<ScheduledCommand, object>>[]
            {
                x => x.DueDate,
                x => x.Failed,
                x => x.ProcessedDateTime!,
            };
            For<ScheduledCommand>().Index(columns);
        }
    }
}
