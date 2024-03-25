using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;

        public ProjectCreatedEventHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = ProjectExtraWorkAgreementList.Create(notification.ProjectId, ProjectExtraWorkAgreementListId.Create(Guid.NewGuid()));
            
            await _projectExtraWorkAgreementListRepository.Add(extraWorkAgreementList, cancellationToken);
            await _projectExtraWorkAgreementListRepository.SaveChanges(cancellationToken);
        }
    }
}
