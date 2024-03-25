using Marten;

namespace deftq.BuildingBlocks.Integration.Outbox.Marten
{
    internal sealed class MartenOutbox : IOutbox
    {
        private readonly IDocumentSession session;

        public MartenOutbox(IDocumentSession session)
        {
            this.session = session;
        }

        public void Add(OutboxMessage message)
        {
            session.Insert(message);
        }

        public IEnumerable<OutboxMessage> GetUnProcessedMessages()
        {
            return session.Query<OutboxMessage>().Where(x => x.ProcessedDate == null).ToList();
        }

        public void Processed(IEnumerable<OutboxMessage> messages)
        {
            session.Update(messages);
        }

        public Task Save()
        {
            return Task.CompletedTask;
        }
    }
}
