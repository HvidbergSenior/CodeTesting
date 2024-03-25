using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public class ProjectRemovedEventHandler  : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public ProjectRemovedEventHandler(IProjectFolderRootRepository projectFolderRootRepository, IUnitOfWork unitOfWork)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var folderRoot = await _projectFolderRootRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            await _projectFolderRootRepository.Delete(folderRoot, cancellationToken);
            await _projectFolderRootRepository.SaveChanges(cancellationToken);
        }
    }
}
