using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public class WeekClosedEventHandler : IDomainEventListener<ProjectLogBookWeekClosedDomainEvent>
    {
        private readonly IProjectLogBookRepository _projectLogBookRepository;

        public WeekClosedEventHandler(IProjectLogBookRepository projectLogBookRepository)
        {
            _projectLogBookRepository = projectLogBookRepository;
        }
        
        public async Task Handle(ProjectLogBookWeekClosedDomainEvent notification, CancellationToken cancellationToken)
        {
            var logBook = await _projectLogBookRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            logBook.SumClosedHours(notification.User);
            await _projectLogBookRepository.Update(logBook, cancellationToken);
            await _projectLogBookRepository.SaveChanges(cancellationToken);
        }
    }
}
