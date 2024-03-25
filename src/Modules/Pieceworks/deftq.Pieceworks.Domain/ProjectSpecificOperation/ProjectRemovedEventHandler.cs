using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public class ProjectRemovedEventHandler : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IProjectSpecificOperationListRepository _projectSpecificOperationListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectRemovedEventHandler(IProjectSpecificOperationListRepository projectSpecificOperationListRepository, IUnitOfWork unitOfWork)
        {
            _projectSpecificOperationListRepository = projectSpecificOperationListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var extraWorkAgreementList = await _projectSpecificOperationListRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            await _projectSpecificOperationListRepository.Delete(extraWorkAgreementList, cancellationToken);
            await _projectSpecificOperationListRepository.SaveChanges(cancellationToken);
        }
    }
}
