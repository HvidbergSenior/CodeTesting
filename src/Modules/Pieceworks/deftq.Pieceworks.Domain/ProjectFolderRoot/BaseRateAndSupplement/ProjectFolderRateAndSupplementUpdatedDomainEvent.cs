using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public sealed class ProjectFolderRateAndSupplementUpdatedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }

        private ProjectFolderRateAndSupplementUpdatedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
        }

        public static ProjectFolderRateAndSupplementUpdatedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            return new ProjectFolderRateAndSupplementUpdatedDomainEvent(projectId, projectFolderId);
        }
    }
}
