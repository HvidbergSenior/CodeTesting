namespace deftq.BuildingBlocks.Integration.Scheduled
{
    public interface ICommandScheduler
    {
        Task Enqueue(IScheduledCommand command, CancellationToken cancellationToken);

        Task Enqueue(IScheduledCommand command, DateTime due, CancellationToken cancellationToken);
    }
}
