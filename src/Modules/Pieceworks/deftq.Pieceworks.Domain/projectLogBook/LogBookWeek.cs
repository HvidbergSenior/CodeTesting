using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookWeek : ValueObject
    {
        public int Value { get; private set; }

        private LogBookWeek()
        {
            Value = 0;
        }

        private LogBookWeek(int value)
        {
            Value = value;
        }

        public static LogBookWeek Create(int value)
        {
            return new LogBookWeek(value);
        }

        public static LogBookWeek Empty()
        {
            return new LogBookWeek(0);
        }
    }
}
