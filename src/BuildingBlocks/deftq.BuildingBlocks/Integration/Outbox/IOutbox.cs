namespace deftq.BuildingBlocks.Integration.Outbox
{
    public interface IOutbox
    {
        void Add(OutboxMessage message);

        IEnumerable<OutboxMessage> GetUnProcessedMessages();

        void Processed(IEnumerable<OutboxMessage> messages);

        Task Save();
    }
}
