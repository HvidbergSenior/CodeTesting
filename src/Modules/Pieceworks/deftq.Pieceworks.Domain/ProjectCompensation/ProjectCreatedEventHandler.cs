using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectCompensationListRepository _projectCompensationListRepository;

        public ProjectCreatedEventHandler(IProjectCompensationListRepository projectCompensationListRepository)
        {
            _projectCompensationListRepository = projectCompensationListRepository;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var projectCompensationList = ProjectCompensationList.Create(notification.ProjectId, ProjectCompensationListId.Create(Guid.NewGuid()));
            await _projectCompensationListRepository.Add(projectCompensationList, cancellationToken);
            await _projectCompensationListRepository.SaveChanges(cancellationToken);
        }
    }
}
