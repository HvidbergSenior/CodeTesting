using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectExtraWorkAgreementId()
        {
            Value = Guid.Empty;
        }

        private ProjectExtraWorkAgreementId(Guid value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementId Create(Guid value)
        {
            return new ProjectExtraWorkAgreementId(value);
        }

        public static ProjectExtraWorkAgreementId Empty()
        {
            return new ProjectExtraWorkAgreementId(Guid.Empty);
        }
    }
}
