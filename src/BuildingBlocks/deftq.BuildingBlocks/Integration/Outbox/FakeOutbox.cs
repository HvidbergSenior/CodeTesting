namespace deftq.BuildingBlocks.Integration.Outbox
{
    public class FakeOutbox : IOutbox
    {
        public IDictionary<Guid, OutboxMessage> Messages { get; }

        public FakeOutbox()
        {
            Messages = new Dictionary<Guid, OutboxMessage>();
        }

        public void Add(OutboxMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Messages.Add(message.Id, message);
        }

        public IEnumerable<OutboxMessage> GetUnProcessedMessages()
        {
            return Messages.Values.Where(x => x.ProcessedDate == null).ToList();
        }

        public void Processed(IEnumerable<OutboxMessage> messages)
        {
            if (messages is null)
            {
                throw new ArgumentNullException(nameof(messages));
            }
            foreach (var item in messages)
            {
                Messages.Add(item.Id, item);
            }
        }

        public Task Save()
        {
            return Task.CompletedTask;
        }
    }
}
