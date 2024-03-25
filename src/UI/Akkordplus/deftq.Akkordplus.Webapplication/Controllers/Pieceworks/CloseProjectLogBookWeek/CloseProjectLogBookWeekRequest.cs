namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CloseProjectLogBookWeek
{
    public class CloseProjectLogBookWeekRequest
    {
        public Guid UserId { get; private set; }
        public int Year { get; private set; }
        public int Week { get; private set; }
        
        public CloseProjectLogBookWeekRequest(Guid userId, int year, int week)
        {
            UserId = userId;
            Year = year;
            Week = week;
        }
    }
}
