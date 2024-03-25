using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookMinutes : ValueObject
    {
        public int Value { get; private set; }

        private LogBookMinutes()
        {
            Value = 0;
        }

        private LogBookMinutes(int value)
        {
            Value = value;
        }

        public static LogBookMinutes Create(int value)
        {
            if (value < 0 || value > 59)
            {
                throw new ArgumentException("Minutes must be between 0 and 59", nameof(value));
            }
            
            return new LogBookMinutes(value);
        }

        public static LogBookMinutes Empty()
        {
            return new LogBookMinutes(0);
        }
    }
}
