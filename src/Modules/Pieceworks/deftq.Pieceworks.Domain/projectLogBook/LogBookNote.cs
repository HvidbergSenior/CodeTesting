using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookNote : ValueObject
    {
        public string Value { get; private set; }

        private LogBookNote()
        {
            Value = string.Empty;
        }

        private LogBookNote(string value)
        {
            Value = value;
        }

        public static LogBookNote Create(string value)
        {
            return new LogBookNote(value);
        }

        public static LogBookNote Empty()
        {
            return new LogBookNote(string.Empty);
        }
    }
}
