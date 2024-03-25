using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectLogBook
{
    public sealed class LogBookTime : ValueObject
    {
        public TimeSpan Value { get; private set; }
        
        private LogBookTime()
        {
            Value = TimeSpan.Zero;
        }
        
        private LogBookTime(TimeSpan value)
        {
            Value = value;
        }
        
        private LogBookTime(LogBookHours hours, LogBookMinutes minutes)
        {
            Value = new TimeSpan(hours.Value, minutes.Value, 0);
        }

        public static LogBookTime Create(LogBookHours hours, LogBookMinutes minutes)
        {
            return new LogBookTime(hours, minutes);
        }
        
        public static LogBookTime Empty()
        {
            return new LogBookTime(LogBookHours.Empty(), LogBookMinutes.Empty());
        }
        
        public LogBookHours GetHours()
        {
            return LogBookHours.Create((int)Value.TotalHours);
        }

        public LogBookMinutes GetMinutes()
        {
            return LogBookMinutes.Create(Value.Minutes);
        }

        public LogBookTime Add(LogBookTime other)
        {
            return new LogBookTime(Value.Add(other.Value));
        }
    }
}
