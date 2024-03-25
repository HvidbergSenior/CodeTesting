using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookHours : ValueObject
    {
        public int Value { get; private set; }

        private LogBookHours()
        {
            Value = 0;
        }

        private LogBookHours(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Hours must be 0 or greater", nameof(value));
            }
            
            Value = value;
        }

        public static LogBookHours Create(int value)
        {
            return new LogBookHours(value);
        }

        public static LogBookHours Empty()
        {
            return new LogBookHours(0);
        }
    }
}
