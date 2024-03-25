using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookSalaryAdvanceAmount : ValueObject
    {
        public decimal Value { get; private set; }

        private LogBookSalaryAdvanceAmount()
        {
            Value = 0;
        }

        private LogBookSalaryAdvanceAmount(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value),"Log book salary advance value must be greater than or equal to 0");
            }
            Value = value;
        }

        public static LogBookSalaryAdvanceAmount Create(decimal value)
        {
            return new LogBookSalaryAdvanceAmount(value);
        }

        public static LogBookSalaryAdvanceAmount Empty()
        {
            return new LogBookSalaryAdvanceAmount(0);
        }
    }
}
