using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public sealed class ProjectCompensationDate : ValueObject
    {
        public DateOnly Value { get; private set; }

        private ProjectCompensationDate()
        {
            Value = DateOnly.FromDateTime(DateTime.MinValue);
        }

        private ProjectCompensationDate(DateOnly value)
        {
            Value = value;
        }

        public static ProjectCompensationDate Create(DateOnly value)
        {
            return new ProjectCompensationDate(value);
        }

        public static ProjectCompensationDate Empty()
        {
            return new ProjectCompensationDate(DateOnly.MinValue);
        }
    }
}
