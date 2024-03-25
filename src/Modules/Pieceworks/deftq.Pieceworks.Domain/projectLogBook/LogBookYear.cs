using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookYear : ValueObject
    {
        public int Value { get; private set; }

        private LogBookYear()
        {
            Value = 0;
        }

        private LogBookYear(int value)
        {
            Value = value;
        }

        public static LogBookYear Create(int value)
        {
            return new LogBookYear(value);
        }

        public static LogBookYear Empty()
        {
            return new LogBookYear(0);
        }
    }
}
