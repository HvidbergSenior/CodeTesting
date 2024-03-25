using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class ProjectRemovedEventHandler : IDomainEventListener<ProjectRemovedDomainEvent>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectRemovedEventHandler(IProjectFolderWorkRepository projectFolderWorkRepository, IUnitOfWork unitOfWork)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProjectRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var folderWorks = await _projectFolderWorkRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);

            foreach (var folderWork in folderWorks)
            {
                await _projectFolderWorkRepository.Delete(folderWork, cancellationToken);
            }

            await _projectFolderWorkRepository.SaveChanges(cancellationToken);
        }
    }
}
