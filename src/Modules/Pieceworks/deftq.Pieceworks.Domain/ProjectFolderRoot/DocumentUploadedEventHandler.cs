using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.projectDocument;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    internal class DocumentUploadedEventHandler : IDomainEventListener<ProjectDocumentUploadedDomainEvent>
    {
        private readonly IProjectFolderRootRepository _projectFolderRootRepository;

        public DocumentUploadedEventHandler(IProjectFolderRootRepository projectFolderRootRepository)
        {
            _projectFolderRootRepository = projectFolderRootRepository;
        }

        public async Task Handle(ProjectDocumentUploadedDomainEvent notification, CancellationToken cancellationToken)
        {
            var projectFolderRoot = await _projectFolderRootRepository.GetByProjectId(notification.ProjectId.Value, cancellationToken);
            if (notification.ProjectFolderId is null)
            {
                // To be fixed in DevOps task #9347
                throw new NotSupportedException("Only documents in folders are supported");
            }
            else
            {
                projectFolderRoot.AddDocument(notification.ProjectFolderId, notification.ProjectDocumentId, notification.ProjectDocumentName, notification.UploadedTimestamp);
            }

            await _projectFolderRootRepository.Update(projectFolderRoot, cancellationToken);
        }
    }
}
