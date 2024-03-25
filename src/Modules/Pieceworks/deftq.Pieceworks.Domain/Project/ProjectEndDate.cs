namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectEndDate
    {
        public DateOnly Value { get; private set; }

        private ProjectEndDate()
        {
            Value = DateOnly.FromDateTime(DateTime.MinValue);
        }

        private ProjectEndDate(DateOnly value)
        {
            Value = value;
        }

        public static ProjectEndDate Create(DateOnly value)
        {
            return new ProjectEndDate(value);
        }

        public static ProjectEndDate Empty()
        {
            return new ProjectEndDate(DateOnly.MinValue);
        }
    }
}
