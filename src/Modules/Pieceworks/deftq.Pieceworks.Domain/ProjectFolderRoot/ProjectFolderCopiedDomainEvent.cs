using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderCopiedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id of project entity
        /// </summary>
        public ProjectId ProjectId { get; }
        
        /// <summary>
        /// Id of the folder copy. This is the new folder that was created as a copy of folder with id <see cref="Source"/>
        /// </summary>
        public ProjectFolderId Copy { get; }
        
        /// <summary>
        /// Id of source folder. This is the folder which a copy was made of
        /// </summary>
        public ProjectFolderId Source { get; }
        
        /// <summary>
        /// Id of destination folder. The destination folder contains the newly created copy
        /// </summary>
        public ProjectFolderId To { get; }

        private ProjectFolderCopiedDomainEvent(ProjectId projectId, ProjectFolderId copy, ProjectFolderId source, ProjectFolderId to)
        {
            ProjectId = projectId;
            Copy = copy;
            Source = source;
            To = to;
        }
        
        public static ProjectFolderCopiedDomainEvent Create(ProjectId projectId, ProjectFolderId copy, ProjectFolderId source, ProjectFolderId to)
        {
            return new ProjectFolderCopiedDomainEvent(projectId, copy, source, to);
        }
    }
}
