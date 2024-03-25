using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class FolderSupplementsDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public IList<FolderSupplement> FolderSupplements { get; }
        
        private FolderSupplementsDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId, IList<FolderSupplement>  folderSupplements)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            FolderSupplements = folderSupplements;
        }

        public static FolderSupplementsDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId, IList<FolderSupplement>  folderSupplements)
        {
            return new FolderSupplementsDomainEvent(projectId, projectFolderId, folderSupplements);
        }
    }
}
