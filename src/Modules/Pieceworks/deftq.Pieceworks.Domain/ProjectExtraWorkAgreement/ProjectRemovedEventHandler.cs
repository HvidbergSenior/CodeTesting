using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public class ProjectRemovedEventHandler : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IProjectExtraWorkAgreementListRepository _projectExtraWorkAgreementListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectRemovedEventHandler(IProjectExtraWorkAgreementListRepository projectExtraWorkAgreementListRepository, IUnitOfWork unitOfWork)
        {
            _projectExtraWorkAgreementListRepository = projectExtraWorkAgreementListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = await _projectExtraWorkAgreementListRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            await _projectExtraWorkAgreementListRepository.Delete(extraWorkAgreementList, cancellationToken);
            await _projectExtraWorkAgreementListRepository.SaveChanges(cancellationToken);
        }
    }
}
