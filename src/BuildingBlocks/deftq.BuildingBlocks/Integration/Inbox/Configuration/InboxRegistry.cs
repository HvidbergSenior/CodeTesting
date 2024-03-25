using Marten;

namespace deftq.BuildingBlocks.Integration.Inbox.Configuration
{
    internal sealed class InboxRegistry : MartenRegistry
    {
        public InboxRegistry()
        {
            const string schemaname = "Integration";

            For<InboxMessage>().DatabaseSchemaName(schemaname);
            For<InboxMessage>().UseOptimisticConcurrency(enabled: true);
            For<InboxMessage>().Index(x => x.OccurredOn);
            For<InboxMessage>().Index(x => x.ProcessedDate!);
            For<InboxMessage>().UniqueIndex(x => x.IntegrationEventId, x => x.MessageType);
        }
    }
}
