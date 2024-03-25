namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookSalaryAdvance
    {
        public LogBookSalaryAdvanceAmount? Amount { get; private set; }
        public LogBookSalaryAdvanceRole? Role { get; private set; }

        private LogBookSalaryAdvance(LogBookSalaryAdvanceAmount amount, LogBookSalaryAdvanceRole role)
        {
            Amount = amount;
            Role = role;
        }
        
        private LogBookSalaryAdvance() {}

        public bool IsEmpty()
        {
            return Amount is null || Role is null;
        }

        public static LogBookSalaryAdvance Create(LogBookSalaryAdvanceAmount amount, LogBookSalaryAdvanceRole role)
        {
            return new LogBookSalaryAdvance(amount, role);
        }

        public static LogBookSalaryAdvance Empty()
        {
            return new LogBookSalaryAdvance();
        }
    }
}
