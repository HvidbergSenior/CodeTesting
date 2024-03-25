namespace deftq.BuildingBlocks.Integration.Scheduled
{
    internal interface ICommandSchedulerRepository
    {
        Task Add(ScheduledCommand cmd, CancellationToken cancellationToken);

        Task<IReadOnlyList<ScheduledCommand>> GetDueForProcessing(CancellationToken cancellationToken);

        Task Update(ScheduledCommand cmd, CancellationToken cancellationToken);
    }
}
