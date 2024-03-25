using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookName : ValueObject
    {
        public string Value { get; private set; }

        private LogBookName()
        {
            Value = string.Empty;
        }

        private LogBookName(string value)
        {
            Value = value;
        }

        public static LogBookName Create(string value)
        {
            return new LogBookName(value);
        }

        public static LogBookName Empty()
        {
            return new LogBookName(string.Empty);
        }
    }
}
