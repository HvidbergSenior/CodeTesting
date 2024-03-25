namespace deftq.BuildingBlocks.Time
{
    public interface ISystemTime
    {
        DateOnly Today();

        DateTimeOffset Now();

        DateTimeOffset DateOnlyAsUtcTimestamp(DateOnly dateOnly);
    }
}
