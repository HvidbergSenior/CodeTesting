using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    [Serializable]
    public class ProjectExtraWorkAgreementNotFoundException : NotFoundException
    {
        public ProjectExtraWorkAgreementNotFoundException() { }
        public ProjectExtraWorkAgreementNotFoundException(string message) : base(message) { }
        public ProjectExtraWorkAgreementNotFoundException(Guid entityId) : base($"Extra work agreement with id {entityId} not found.") { }
        public ProjectExtraWorkAgreementNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectExtraWorkAgreementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
