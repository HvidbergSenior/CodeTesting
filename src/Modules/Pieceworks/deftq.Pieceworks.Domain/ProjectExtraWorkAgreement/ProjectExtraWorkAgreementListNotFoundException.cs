using System.Runtime.Serialization;
using deftq.BuildingBlocks.Exceptions;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    [Serializable]
    public class ProjectExtraWorkAgreementListNotFoundException : NotFoundException
    {
        public ProjectExtraWorkAgreementListNotFoundException() {}
        public ProjectExtraWorkAgreementListNotFoundException(string message) : base(message) {}
        public ProjectExtraWorkAgreementListNotFoundException(Guid entityId) : base($"Extra work agreement list id {entityId} not found."){ }
        public ProjectExtraWorkAgreementListNotFoundException(string message, Exception inner) : base(message, inner) {}
        protected ProjectExtraWorkAgreementListNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context){}
    }
}
