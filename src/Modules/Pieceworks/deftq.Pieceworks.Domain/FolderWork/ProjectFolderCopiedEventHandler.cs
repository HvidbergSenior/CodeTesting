using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class ProjectFolderCopiedEventHandler : IDomainEventListener<ProjectFolderCopiedDomainEvent>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IBaseRateAndSupplementRepository _baseRateAndSupplementRepository;
        private readonly IExecutionContext _executionContext;
        
        public ProjectFolderCopiedEventHandler(IProjectFolderRootRepository projectFolderRootRepository, IProjectFolderWorkRepository projectFolderWorkRepository, IBaseRateAndSupplementRepository baseRateAndSupplementRepository, IExecutionContext executionContext)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _baseRateAndSupplementRepository = baseRateAndSupplementRepository;
            _executionContext = executionContext;
        }
        
        public async Task Handle(ProjectFolderCopiedDomainEvent notification, CancellationToken cancellationToken)
        {
            var sourceFolderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(notification.ProjectId.Value, notification.Source.Value, cancellationToken);
            var destinationFolderWork = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), notification.ProjectId,
                notification.Copy);
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            
            var systemBaseRateAndSupplement = await _baseRateAndSupplementRepository.Get(cancellationToken);
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(systemBaseRateAndSupplement, folderRoot.GetFolder(notification.Copy));
            
            sourceFolderWork.CopyWorkItems(destinationFolderWork, baseRateAndSupplementProxy, sourceFolderWork.WorkItems.Select(workItem => workItem.WorkItemId).ToList(), _executionContext);

            await _projectFolderWorkRepository.Add(destinationFolderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
        }
    }
}
