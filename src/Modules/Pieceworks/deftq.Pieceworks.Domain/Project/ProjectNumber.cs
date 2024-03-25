using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project;

public sealed class ProjectNumber : ValueObject
{
    public long Value { get; private set; }

    private ProjectNumber()
    {
        Value = 0;
    }

    private ProjectNumber(long value)
    {
        Value = value;
    }

    public static ProjectNumber Create(long value)
    {
        return new ProjectNumber(value);
    }

    public static ProjectNumber Empty()
    {
        return new ProjectNumber(0);
    }
}
