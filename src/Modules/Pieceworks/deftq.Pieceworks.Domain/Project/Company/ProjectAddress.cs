using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project.Company
{
    public sealed class ProjectAddress : ValueObject
    {
        public string Value { get; private set; }

        private ProjectAddress()
        {
            Value = String.Empty;
        }

        private ProjectAddress(string value)
        {
            if (value == null || value.Length > 40)
            {
                throw new ArgumentException("", nameof(value));
            }
            Value = value;
        }

        public static ProjectAddress Create(string value)
        {
            return new ProjectAddress(value);
        }

        public static ProjectAddress Empty()
        {
            return new ProjectAddress(String.Empty);
        }
    }
}
