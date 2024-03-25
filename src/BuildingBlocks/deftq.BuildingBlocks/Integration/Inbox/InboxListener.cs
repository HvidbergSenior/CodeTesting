using System.Threading.Channels;

namespace deftq.BuildingBlocks.Integration.Inbox
{
    public class InboxListener
    {
        private readonly Channel<Guid> _messageIdChannel;

        public InboxListener()
        {
            // If the consumer is slow, this should be a bounded channel to avoid memory growing indefinitely.
            // To make an informed decision we should instrument the code and gather metrics.
            _messageIdChannel = Channel.CreateUnbounded<Guid>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false,
                });
        }

        public void OnNewMessages(IEnumerable<Guid> messageIds)
        {
            if (messageIds is null)
            {
                throw new ArgumentNullException(nameof(messageIds));
            }

            foreach (var messageId in messageIds)
            {
                // we don't care too much if it succeeds because we'll have a fallback to handle "forgotten" messages
                if (!_messageIdChannel.Writer.TryWrite(messageId))
                {
                    //_logger.LogDebug("Could not add outbox message {messageId} to the channel.", messageId);
                }
            }
        }

        public IAsyncEnumerable<Guid> GetAllMessageIdsAsync(CancellationToken ct)
            => _messageIdChannel.Reader.ReadAllAsync(ct);
    }
}
