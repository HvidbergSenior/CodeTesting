using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public class ProjectRemovedEventHandler : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IProjectLogBookRepository _projectLogBookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectRemovedEventHandler(IProjectLogBookRepository projectLogBookRepository, IUnitOfWork unitOfWork)
        {
            _projectLogBookRepository = projectLogBookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var logBook =  await _projectLogBookRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            await _projectLogBookRepository.Delete(logBook, cancellationToken);
            await _projectLogBookRepository.SaveChanges(cancellationToken);
        }
    }
}
