using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectOrderNumber : ValueObject
    {
        public String Value { get; private set; }

        private ProjectOrderNumber()
        {
            Value = string.Empty;
        }

        private ProjectOrderNumber(string value)
        {
            Value = value;
        }

        public static ProjectOrderNumber Create(string value)
        {
            if (value.Length > 15)
            {
                throw new ArgumentException("Value cant be longer than 15 chars", nameof(value));
            }

            return new ProjectOrderNumber(value);
        }

        public static ProjectOrderNumber Empty()
        {
            return new ProjectOrderNumber(string.Empty);
        }
    }
}
