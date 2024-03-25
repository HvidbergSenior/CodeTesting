using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project.Company
{
    public sealed class ProjectCompanyName : ValueObject
    {
        public string Value { get; private set; }

        private ProjectCompanyName()
        {
            Value = String.Empty;
        }

        private ProjectCompanyName(string value)
        {
            if (value == null || value.Length > 40)
            {
                throw new ArgumentException("", nameof(value));
            }
            Value = value;
        }

        public static ProjectCompanyName Create(string value)
        {
            return new ProjectCompanyName(value);
        }

        public static ProjectCompanyName Empty()
        {
            return new ProjectCompanyName(String.Empty);
        }
    }
}
