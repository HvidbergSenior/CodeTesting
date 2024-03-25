using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    [Serializable]
    public class ProjectSpecificOperationListNotFoundException : NotFoundException
    {
        public ProjectSpecificOperationListNotFoundException() {}
        public ProjectSpecificOperationListNotFoundException(string message) : base(message) {}
        public ProjectSpecificOperationListNotFoundException(Guid entityId) : base($"Project Specific Operation list id {entityId} not found."){ }
        public ProjectSpecificOperationListNotFoundException(string message, Exception inner) : base(message, inner) {}
        protected ProjectSpecificOperationListNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context){}
    }
}
