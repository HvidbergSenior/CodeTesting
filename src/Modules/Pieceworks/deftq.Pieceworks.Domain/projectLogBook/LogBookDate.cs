using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookDate : ValueObject
    {
        public DateOnly Value { get; private set; }

        private LogBookDate()
        {
            Value = DateOnly.FromDateTime(DateTime.MinValue);
        }

        private LogBookDate(DateOnly value)
        {
            Value = value;
        }

        public static LogBookDate Create(DateOnly value)
        {
            return new LogBookDate(value);
        }
        
        public static LogBookDate Empty()
        {
            return new LogBookDate(DateOnly.MinValue);
        }

        public static LogBookDate Today()
        {
            return new LogBookDate();
        }
    }
}
