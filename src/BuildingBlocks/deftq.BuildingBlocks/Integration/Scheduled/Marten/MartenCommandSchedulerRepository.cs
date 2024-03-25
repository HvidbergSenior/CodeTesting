using System.Diagnostics.CodeAnalysis;
using Marten;

namespace deftq.BuildingBlocks.Integration.Scheduled.Marten
{
    internal sealed class MartenCommandSchedulerRepository : ICommandSchedulerRepository
    {
        private readonly IDocumentSession session;

        public MartenCommandSchedulerRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public Task Add(ScheduledCommand cmd, CancellationToken cancellationToken)
        {
            session.Insert(cmd);
            return Task.CompletedTask;
        }

        [SuppressMessage("Roslynator", "RCS1049:Simplify boolean comparison.", Justification = "<Pending>")]
        public async Task<IReadOnlyList<ScheduledCommand>> GetDueForProcessing(CancellationToken cancellationToken)
        {
            var commands = await session.Query<ScheduledCommand>()
                .Where(x => x.DueDate <= DateTimeOffset.UtcNow.DateTime
                    && x.ProcessedDateTime == null
                    && x.Failed == false
                )
                .OrderBy(x => x.DueDate)
                .Take(5)
                .ToListAsync(cancellationToken);

            return commands;
        }

        public Task Update(ScheduledCommand cmd, CancellationToken cancellationToken)
        {
            session.Update(cmd);
            return Task.CompletedTask;
        }
    }
}
