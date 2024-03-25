using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    [Serializable]
    public class ProjectSpecificOperationNotFoundException : NotFoundException
    {
        public ProjectSpecificOperationNotFoundException() { }
        public ProjectSpecificOperationNotFoundException(string message) : base(message) { }
        public ProjectSpecificOperationNotFoundException(Guid entityId) : base($"Project Specific Operation with id {entityId} not found.") { }
        public ProjectSpecificOperationNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectSpecificOperationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
