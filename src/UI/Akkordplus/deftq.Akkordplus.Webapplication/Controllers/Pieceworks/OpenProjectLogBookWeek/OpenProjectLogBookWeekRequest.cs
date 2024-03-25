namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.OpenProjectLogBookWeek
{
    public class OpenProjectLogBookWeekRequest
    {
        public Guid UserId { get; private set; }
        public int Year { get; private set; }
        public int Week { get; private set; }
        
        public OpenProjectLogBookWeekRequest(Guid userId, int year, int week)
        {
            UserId = userId;
            Year = year;
            Week = week;
        }
    }
}
