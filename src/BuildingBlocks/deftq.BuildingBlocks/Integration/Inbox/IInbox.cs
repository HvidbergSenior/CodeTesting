namespace deftq.BuildingBlocks.Integration.Inbox
{
    public interface IInbox
    {
        void Add(InboxMessage inboxMessage);

        IEnumerable<InboxMessage> GetUnProcessedMessages();

        void Processed(IEnumerable<InboxMessage> messages);
    }
}
