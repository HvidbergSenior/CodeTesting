using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.FolderWork
{
    [Serializable]
    public class WorkItemNotFoundException : NotFoundException
    {
        public WorkItemNotFoundException() { }
        public WorkItemNotFoundException(string message) : base(message) { }
        public WorkItemNotFoundException(Guid entityId) : base($"Work item with id {entityId} not found.") { }
        public WorkItemNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected WorkItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
