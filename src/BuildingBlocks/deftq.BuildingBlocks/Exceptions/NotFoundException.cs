using System.Runtime.Serialization;

namespace deftq.BuildingBlocks.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(Guid entityId) : base($"Entity {entityId} not found.") { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
        protected NotFoundException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

}
