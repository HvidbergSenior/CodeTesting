using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public class ProjectCompensationListNotFoundException : NotFoundException
    {
        public ProjectCompensationListNotFoundException() {}
        public ProjectCompensationListNotFoundException(string message) : base(message) { }
        public ProjectCompensationListNotFoundException(Guid entityId) : base($"Project compensation id {entityId} not found."){ }
        public ProjectCompensationListNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectCompensationListNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
