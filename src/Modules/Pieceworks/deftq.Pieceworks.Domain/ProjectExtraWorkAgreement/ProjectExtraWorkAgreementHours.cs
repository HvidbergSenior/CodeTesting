using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementHours : ValueObject
    {
        public int Value { get; private set; }

        private ProjectExtraWorkAgreementHours()
        {
            Value = 0;
        }

        private ProjectExtraWorkAgreementHours(int value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementHours Create(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Hours can´t be less than zero", nameof(value));
            }
            
            return new ProjectExtraWorkAgreementHours(value);
        }

        public static ProjectExtraWorkAgreementHours Empty()
        {
            return new ProjectExtraWorkAgreementHours(0);
        }
    }
}
