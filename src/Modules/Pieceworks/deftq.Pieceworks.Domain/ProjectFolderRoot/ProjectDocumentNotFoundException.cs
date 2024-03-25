using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.projectDocument;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    [Serializable]
    public class ProjectDocumentNotFoundException : NotFoundException
    {
        public ProjectDocumentNotFoundException() { }

        public ProjectDocumentNotFoundException(string message) : base(message) { }

        public ProjectDocumentNotFoundException(Guid entityId) : base($"Unknown document id {entityId}") { }

        public ProjectDocumentNotFoundException(ProjectDocumentId projectDocumentId) : base(projectDocumentId.Value) { }

        public ProjectDocumentNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected ProjectDocumentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
