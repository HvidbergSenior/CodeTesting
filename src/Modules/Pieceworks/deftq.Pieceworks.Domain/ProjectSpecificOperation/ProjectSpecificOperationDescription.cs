using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperationDescription : ValueObject
    {
        public string Value { get; private set; }

        private ProjectSpecificOperationDescription()
        {
            Value = string.Empty;
        }

        private ProjectSpecificOperationDescription(string value)
        {
            Value = value;
        }

        public static ProjectSpecificOperationDescription Create(string? value)
        {
            if (value is null)
            {
                return Empty();
            }
            return new ProjectSpecificOperationDescription(value);
        }

        public static ProjectSpecificOperationDescription Empty()
        {
            return new ProjectSpecificOperationDescription(string.Empty);
        }
    }
}
