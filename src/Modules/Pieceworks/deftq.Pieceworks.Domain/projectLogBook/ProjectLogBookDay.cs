using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class ProjectLogBookDay : Entity
    {
        public LogBookDate Date { get; private set; }
        public LogBookTime Time { get; private set; }

        private ProjectLogBookDay()
        {
            Id = Guid.NewGuid();
            Date = LogBookDate.Empty();
            Time = LogBookTime.Empty();
        }

        private ProjectLogBookDay(LogBookDate date, LogBookTime time)
        {
            Id = Guid.NewGuid();
            Date = date;
            Time = time;
        }

        public static ProjectLogBookDay Create(LogBookDate date, LogBookTime time)
        {
            if (time.GetHours().Value < 0 || time.GetHours().Value > 24)
            {
                throw new ArgumentException("Hours cant be less than 0 or more then 24", nameof(time));
            }

            if (time.GetMinutes().Value < 0 || time.GetMinutes().Value > 59)
            {
                throw new ArgumentException("Minutes cant be less than 0 or more than 59", nameof(time));
            }

            if (time.GetHours().Value >= 24 && time.GetMinutes().Value > 0)
            {
                throw new ArgumentException("There are no more than 24 hours in a day", nameof(time));
            }
            return new ProjectLogBookDay(date, time);
        }
    }
}
