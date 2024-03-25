using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperationName : ValueObject
    {
        public string Value { get; private set; }

        private ProjectSpecificOperationName()
        {
            Value = string.Empty;
        }

        private ProjectSpecificOperationName(string value)
        {
            Value = value;
        }

        public static ProjectSpecificOperationName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is empty", nameof(value));
            }

            if (value.Length > 40)
            {
                throw new ArgumentException("Value cant be longer than 40 chars", nameof(value));
            }

            return new ProjectSpecificOperationName(value);
        }

        public static ProjectSpecificOperationName Empty()
        {
            return new ProjectSpecificOperationName(string.Empty);
        }
    }
}
