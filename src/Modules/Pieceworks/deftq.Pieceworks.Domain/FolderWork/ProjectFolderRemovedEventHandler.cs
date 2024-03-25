using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class ProjectFolderRemovedEventHandler : IDomainEventListener<ProjectFolderRemovedDomainEvent>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;

        public ProjectFolderRemovedEventHandler(IProjectFolderWorkRepository projectFolderWorkRepository)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
        }

        public async Task Handle(ProjectFolderRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var folderWork = await _projectFolderWorkRepository.GetByProjectAndFolderId(notification.ProjectId.Value, notification.ProjectFolderId.Value, cancellationToken);
            await _projectFolderWorkRepository.Delete(folderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
        }
    }
}
