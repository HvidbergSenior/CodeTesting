using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class ProjectCreatedEventHandler : IDomainEventListener<ProjectCreatedDomainEvent>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;

        public ProjectCreatedEventHandler(IProjectFolderWorkRepository projectFolderWorkRepository)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var folderWork = ProjectFolderWork.Create(ProjectFolderWorkId.Create(Guid.NewGuid()), notification.ProjectId, ProjectFolderRoot.RootFolderId);
            await _projectFolderWorkRepository.Add(folderWork, cancellationToken);
            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
        }
    }
}
