using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectCreatedTime : ValueObject
    {
        public DateTimeOffset Value { get; private set; }

        private ProjectCreatedTime()
        {
            Value = DateTimeOffset.MinValue;
        }

        private ProjectCreatedTime(DateTimeOffset value)
        {
            Value = value;
        }

        public static ProjectCreatedTime Create(DateTimeOffset value)
        {
            return new ProjectCreatedTime(value);
        }

        public static ProjectCreatedTime Empty()
        {
            return new ProjectCreatedTime();
        }
    }
}