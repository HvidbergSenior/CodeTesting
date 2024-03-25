using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterLogbookSalaryAdvance
{
    public class RegisterLogbookSalaryAdvanceRequest
    {
        public Guid UserId { get; private set; }
        public int Year { get; private set; }
        public int Week { get; private set; }
        public LogBookSalaryAdvanceRoleRequest Type { get; private set; }
        public decimal Amount { get; private set; }

        public RegisterLogbookSalaryAdvanceRequest(Guid userId, int year, int week, LogBookSalaryAdvanceRoleRequest type, decimal amount)
        {
            UserId = userId;
            Week = week;
            Year = year;
            Amount = amount;
            Type = type;
        }
    }
    
    public enum LogBookSalaryAdvanceRoleRequest
    {
        Participant = 0,
        Apprentice = 1
    }
}
