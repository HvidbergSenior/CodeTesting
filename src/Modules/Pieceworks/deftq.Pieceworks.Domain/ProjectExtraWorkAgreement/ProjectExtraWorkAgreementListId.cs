using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementListId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectExtraWorkAgreementListId()
        {
            Value = Guid.Empty;
        }

        private ProjectExtraWorkAgreementListId(Guid value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementListId Create(Guid value)
        {
            return new ProjectExtraWorkAgreementListId(value);
        }

        public static ProjectExtraWorkAgreementListId Empty()
        {
            return new ProjectExtraWorkAgreementListId(Guid.Empty);
        }
    }
}
