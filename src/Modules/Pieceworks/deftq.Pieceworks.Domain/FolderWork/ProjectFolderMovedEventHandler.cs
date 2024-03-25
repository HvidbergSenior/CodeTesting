using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class ProjectFolderMovedEventHandler : IDomainEventListener<ProjectFolderMovedDomainEvent>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IExecutionContext _executionContext;

        public ProjectFolderMovedEventHandler(IProjectFolderRootRepository projectFolderRootRepository,
            IProjectFolderWorkRepository projectFolderWorkRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository,
            IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _executionContext = executionContext;
        }

        public async Task Handle(ProjectFolderMovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);

            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);

            var projectFolder = folderRoot.GetFolder(notification.ProjectFolderId);
            foreach (var folder in projectFolder.GetFolderAndSubFolders())
            {
                var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, folder);
                var calc = new WorkItemCalculator(baseRateAndSupplementProxy);
                
                var folderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(notification.ProjectId.Value,
                    folder.ProjectFolderId.Value,
                    cancellationToken);
                
                foreach (var workItem in folderWork.WorkItems)
                {
                    var result = calc.CalculateTotalOperationTime(workItem);
                    workItem.UpdateTotalOperationTime(result);
                }
                
                await _projectFolderWorkRepository.Update(folderWork, cancellationToken);
            }
            
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
        }
    }
}
