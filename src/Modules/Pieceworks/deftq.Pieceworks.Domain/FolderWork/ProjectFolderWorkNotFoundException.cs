using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.FolderWork
{
    [Serializable]
    public class ProjectFolderWorkNotFoundException : NotFoundException
    {
        public ProjectFolderWorkNotFoundException() { }
        public ProjectFolderWorkNotFoundException(string message) : base(message) { }
        public ProjectFolderWorkNotFoundException(Guid entityId) : base($"FolderWork with id {entityId} not found.") { }
        public ProjectFolderWorkNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectFolderWorkNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
