using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectSpecificOperationListRepository _projectSpecificOperationListRepository;

        public ProjectCreatedEventHandler(IProjectSpecificOperationListRepository projectSpecificOperationListRepository)
        {
            _projectSpecificOperationListRepository = projectSpecificOperationListRepository;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = ProjectSpecificOperationList.Create(notification.ProjectId, ProjectSpecificOperationListId.Create(Guid.NewGuid()));
            
            await _projectSpecificOperationListRepository.Add(extraWorkAgreementList, cancellationToken);
            await _projectSpecificOperationListRepository.SaveChanges(cancellationToken);
        }
    }
}
