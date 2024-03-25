namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectLogBookWeek
{
    public class RegisterProjectLogBookWeekRequest
    {
        public Guid UserId { get; private set; }
        public int Year { get; private set; }
        public int Week { get; private set; }
        public string Note { get; private set; }
        public IList<RegisterProjectLogBookDay> Days { get; private set; }

        public RegisterProjectLogBookWeekRequest(Guid userId, int year, int week, string note, IList<RegisterProjectLogBookDay> days)
        {
            UserId = userId;
            Year = year;
            Week = week;
            Note = note;
            Days = days;
        }
    }

    public class RegisterProjectLogBookDay
    {
        public DateTimeOffset Date { get; private set; }
        public int Hours { get; private set; }
        public int Minutes { get; private set; }

        public RegisterProjectLogBookDay(DateTimeOffset date, int hours, int minutes)
        {
            Date = date;
            Hours = hours;
            Minutes = minutes;
        }
    }
}
