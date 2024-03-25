using Marten;

namespace deftq.BuildingBlocks.Integration.Outbox.Configuration
{
    internal sealed class OutboxRegistry : MartenRegistry
    {
        public OutboxRegistry()
        {
            const string schemaname = "Integration";

            For<OutboxMessage>().DatabaseSchemaName(schemaname);
            For<OutboxMessage>().UseOptimisticConcurrency(enabled: true);
            For<OutboxMessage>().Index(x => x.OccurredOn);
            For<OutboxMessage>().Index(x => x.ProcessedDate!);
        }
    }
}
