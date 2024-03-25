using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    [Serializable]
    public class ProjectFolderNotFoundException : NotFoundException
    {
        public ProjectFolderNotFoundException() { }
        
        public ProjectFolderNotFoundException(string message) : base(message) { }

        public ProjectFolderNotFoundException(Guid entityId) : base($"Unknown project folder id {entityId}") { }

        public ProjectFolderNotFoundException(ProjectFolderId projectFolderId) : base(projectFolderId.Value) { }

        public ProjectFolderNotFoundException(string message, Exception inner) : base(message, inner) { }
        
        protected ProjectFolderNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
