using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class ProjectFolderAddedEventHandler : IDomainEventListener<ProjectFolderAddedDomainEvent>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;

        public ProjectFolderAddedEventHandler(IProjectFolderWorkRepository projectFolderWorkRepository)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
        }

        public async Task Handle(ProjectFolderAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var folderWork = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), notification.ProjectId, notification.ProjectFolderId);
            await _projectFolderWorkRepository.Update(folderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
        }
    }
}
