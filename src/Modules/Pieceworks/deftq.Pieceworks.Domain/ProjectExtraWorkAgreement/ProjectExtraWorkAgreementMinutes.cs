using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementMinutes : ValueObject
    {
        public int Value { get; private set; }

        private ProjectExtraWorkAgreementMinutes()
        {
            Value = 0;
        }

        private ProjectExtraWorkAgreementMinutes(int value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementMinutes Create(int value)
        {
            if (value is < 0 or > 59)
            {
                throw new ArgumentException("Minutes must between 0 and 59", nameof(value));
            }
            
            return new ProjectExtraWorkAgreementMinutes(value);
        }

        public static ProjectExtraWorkAgreementMinutes Empty()
        {
            return new ProjectExtraWorkAgreementMinutes(0);
        }
    }
}
