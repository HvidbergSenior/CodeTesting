using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Application.GetProjectLogBookWeek
{
    public class GetProjectLogBookWeekQueryResponse
    {
        public int Year { get; private set; }
        public int Week { get; private set; }
        public string UserName { get; private set; }
        public string Note { get; private set; }
        public bool Closed { get; private set; }
        public LogBookTimeResponse WeekSummation { get; private set; }
        public LogBookTimeResponse ClosedWeeksSummation { get; private set; }
        public IList<GetProjectLogBookDayResponse> Days { get; private set; }

        public LogBookSalaryAdvanceResponse SalaryAdvance { get; private set; }

        private GetProjectLogBookWeekQueryResponse()
        {
            Year = 0;
            Week = 0;
            UserName = String.Empty;
            Note = String.Empty;
            Closed = false;
            Days = new List<GetProjectLogBookDayResponse>();
            WeekSummation = LogBookTimeResponse.Empty();
            ClosedWeeksSummation = LogBookTimeResponse.Empty();
            SalaryAdvance = LogBookSalaryAdvanceResponse.Empty();
        }

        public GetProjectLogBookWeekQueryResponse(int year, int week, string userName, string note, bool closed, LogBookTimeResponse weekSummation,
            LogBookTimeResponse closedWeeksSummation, IList<GetProjectLogBookDayResponse> days, LogBookSalaryAdvanceResponse salaryAdvance)
        {
            Year = year;
            Week = week;
            UserName = userName;
            Note = note;
            Closed = closed;
            Days = days;
            WeekSummation = weekSummation;
            ClosedWeeksSummation = closedWeeksSummation;
            SalaryAdvance = salaryAdvance;
        }
    }

    public class GetProjectLogBookDayResponse
    {
        public DateTimeOffset Date { get; private set; }
        public LogBookTimeResponse Time { get; private set; }

        private GetProjectLogBookDayResponse()
        {
            Date = DateTimeOffset.MinValue;
            Time = LogBookTimeResponse.Empty();
        }

        public GetProjectLogBookDayResponse(DateTimeOffset date, LogBookTimeResponse time)
        {
            Date = date;
            Time = time;
        }
    }

    public class LogBookTimeResponse
    {
        public int Hours { get; private set; }
        public int Minutes { get; private set; }

        private LogBookTimeResponse()
        {
            Hours = 0;
            Minutes = 0;
        }

        public LogBookTimeResponse(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public static LogBookTimeResponse Create(LogBookTime time)
        {
            return new LogBookTimeResponse(time.GetHours().Value, time.GetMinutes().Value);
        }

        public static LogBookTimeResponse Empty()
        {
            return new LogBookTimeResponse(0, 0);
        }
    }

    public class LogBookSalaryAdvanceResponse
    {
        public LogBookSalaryAdvanceTimeResponse? Start { get; private set; }
        public LogBookSalaryAdvanceTimeResponse? End { get; private set; }
        public decimal Amount { get; private set; }
        public LogBookSalaryAdvanceRoleResponse Role { get; private set; }

        private LogBookSalaryAdvanceResponse()
        {
            Start = null;
            End = null;
            Amount = 0;
            Role = LogBookSalaryAdvanceRoleResponse.Undefined;
        }
        
        public LogBookSalaryAdvanceResponse(LogBookSalaryAdvanceTimeResponse? start, LogBookSalaryAdvanceTimeResponse? end, decimal amount, LogBookSalaryAdvanceRoleResponse role)
        {
            Start = start;
            End = end;
            Amount = amount;
            Role = role;
        }

        public static LogBookSalaryAdvanceResponse Empty()
        {
            return new LogBookSalaryAdvanceResponse(null, null, 0, LogBookSalaryAdvanceRoleResponse.Undefined);
        }
    }
    
    public enum LogBookSalaryAdvanceRoleResponse {
        Participant,
        Apprentice,
        Undefined
    }

    public class LogBookSalaryAdvanceTimeResponse
    {
        public int Year { get; private set; }
        public int Week { get; private set; }

        private LogBookSalaryAdvanceTimeResponse()
        {
            Year = 0;
            Week = 0;
        }
        
        public LogBookSalaryAdvanceTimeResponse(int year, int week)
        {
            Year = year;
            Week = week;
        }
    }
}
