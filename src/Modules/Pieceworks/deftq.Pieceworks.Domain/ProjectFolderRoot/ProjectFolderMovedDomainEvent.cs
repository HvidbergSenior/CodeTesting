using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderMovedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public ProjectFolderId From { get; }
        public ProjectFolderId To { get; }

        private ProjectFolderMovedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderId from, ProjectFolderId to)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            From = from;
            To = to;
        }

        public static ProjectFolderMovedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderId from, ProjectFolderId to)
        {
            return new ProjectFolderMovedDomainEvent(projectId, projectFolderId, from, to);
        }
    }
}
