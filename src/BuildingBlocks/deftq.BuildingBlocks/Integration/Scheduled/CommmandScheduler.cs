namespace deftq.BuildingBlocks.Integration.Scheduled
{
    internal sealed class CommmandScheduler : ICommandScheduler
    {
        private readonly ICommandSchedulerRepository repository;

        public CommmandScheduler(ICommandSchedulerRepository repository)
        {
            this.repository = repository;
        }

        public Task Enqueue(IScheduledCommand command, CancellationToken cancellationToken)
        {
            return Enqueue(command, DateTime.UtcNow, cancellationToken);
        }

        public Task Enqueue(IScheduledCommand command, DateTime due, CancellationToken cancellationToken)
        {
            var commandType = command.GetType().AssemblyQualifiedName;
            var type = Type.GetType(commandType!);

            var serializer = new ScheduledCommandSerializer();
            var payload = serializer.Serialize(command, type);
            var cmd = ScheduledCommand.With(commandType!, payload, due);
            return repository.Add(cmd, cancellationToken);
        }
    }
}
