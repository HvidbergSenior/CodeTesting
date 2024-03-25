using Marten;

namespace deftq.BuildingBlocks.Integration.Inbox.Marten
{
    internal sealed class MartenInbox : IInbox
    {
        private readonly IDocumentSession documentSession;

        public MartenInbox(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Add(InboxMessage inboxMessage)
        {
            documentSession.Insert(inboxMessage);
        }

        public IEnumerable<InboxMessage> GetUnProcessedMessages()
        {
            return documentSession.Query<InboxMessage>()
                .Where(x => x.ProcessedDate == null)
                .OrderBy(x => x.OccurredOn)
                .ToList();
        }

        public void Processed(IEnumerable<InboxMessage> messages)
        {
            documentSession.Update(messages);
        }
    }
}
