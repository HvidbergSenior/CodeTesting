using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public class WeekOpenedEventHandler : IDomainEventListener<ProjectLogBookWeekOpenedDomainEvent>
    {
        private readonly IProjectLogBookRepository _projectLogBookRepository;

        public WeekOpenedEventHandler(IProjectLogBookRepository projectLogBookRepository)
        {
            _projectLogBookRepository = projectLogBookRepository;
        }
        
        public async Task Handle(ProjectLogBookWeekOpenedDomainEvent notification, CancellationToken cancellationToken)
        {
            var logBook = await _projectLogBookRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            logBook.SumClosedHours(notification.User);
            await _projectLogBookRepository.Update(logBook, cancellationToken);
            await _projectLogBookRepository.SaveChanges(cancellationToken);
        }
    }
}
