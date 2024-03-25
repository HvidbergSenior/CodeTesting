using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class ProjectFolderSupplementsUpdatedEventHandler : IDomainEventListener<ProjectFolderSupplementsUpdatedDomainEvent>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;

        public ProjectFolderSupplementsUpdatedEventHandler(IProjectFolderWorkRepository projectFolderWorkRepository,
            IProjectFolderRootRepository projectFolderRootRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _projectFolderRootRepository = projectFolderRootRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
        }

        public async Task Handle(ProjectFolderSupplementsUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var projectId = notification.ProjectId.Value;
            var folderId = notification.ProjectFolderId.Value;
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(projectId, cancellationToken);
            var folderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(projectId, folderId, cancellationToken);
            
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var calc = new WorkItemCalculator(new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, folderRoot.GetFolder(notification.ProjectFolderId)));
            
            foreach (var workItem in folderWork.WorkItems)
            {
                var result = calc.CalculateTotalOperationTime(workItem);
                workItem.UpdateTotalOperationTime(result);
            }
            
            await _projectFolderWorkRepository.Update(folderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
        }
    }
}
