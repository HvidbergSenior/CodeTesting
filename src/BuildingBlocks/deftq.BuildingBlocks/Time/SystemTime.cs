namespace deftq.BuildingBlocks.Time
{
    public class SystemTime : ISystemTime
    {
        public DateOnly Today()
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }

        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
        
        public DateTimeOffset DateOnlyAsUtcTimestamp(DateOnly dateOnly)
        {
            return new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromHours(12)), DateTimeKind.Local));
        }
    }
}
