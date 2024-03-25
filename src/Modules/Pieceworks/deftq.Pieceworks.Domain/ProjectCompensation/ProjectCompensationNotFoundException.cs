using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public class ProjectCompensationNotFoundException : NotFoundException
    {
        public ProjectCompensationNotFoundException() {}
        public ProjectCompensationNotFoundException(string message) : base(message) { }
        public ProjectCompensationNotFoundException(Guid entityId) : base($"Project compensation id {entityId} not found."){ }
        public ProjectCompensationNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectCompensationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
