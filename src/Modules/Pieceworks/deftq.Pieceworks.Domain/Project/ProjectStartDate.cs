namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectStartDate
    {
        public DateOnly Value { get; private set; }

        private ProjectStartDate()
        {
            Value = DateOnly.FromDateTime(DateTime.MinValue);
        }

        private ProjectStartDate(DateOnly value)
        {
            Value = value;
        }

        public static ProjectStartDate Create(DateOnly value)
        {
            return new ProjectStartDate(value);
        }

        public static ProjectStartDate Empty()
        {
            return new ProjectStartDate(DateOnly.MinValue);
        }
    }
}
