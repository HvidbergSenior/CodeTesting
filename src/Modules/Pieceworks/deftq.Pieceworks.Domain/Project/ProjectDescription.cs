using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectDescription : ValueObject
    {
        public String Value { get; private set; }

        private ProjectDescription()
        {
            Value = string.Empty;
        }

        private ProjectDescription(string value)
        {
            Value = value;
        }

        public static ProjectDescription Create(string value)
        {
            return new ProjectDescription(value);
        }

        public static ProjectDescription Empty()
        {
            return new ProjectDescription(string.Empty);
        }
    }
}
