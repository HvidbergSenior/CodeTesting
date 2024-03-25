using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public class ProjectRemovedEventHandler : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IProjectCompensationListRepository _projectCompensationListRepository;

        public ProjectRemovedEventHandler(IProjectCompensationListRepository projectCompensationListRepository)
        {
            _projectCompensationListRepository = projectCompensationListRepository;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var projectCompensationList = await _projectCompensationListRepository.GetByProjectId(notification.ProjectId.Value);

            await _projectCompensationListRepository.Delete(projectCompensationList, cancellationToken);
            await _projectCompensationListRepository.SaveChanges(cancellationToken);
        }
    }
}
