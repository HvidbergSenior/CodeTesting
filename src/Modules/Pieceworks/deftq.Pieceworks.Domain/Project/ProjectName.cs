using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project;

public sealed class ProjectName : ValueObject
{
    public String Value { get; private set; }

    private ProjectName()
    {
        Value = string.Empty;
    }

    private ProjectName(string value)
    {
        Value = value;
    }

    public static ProjectName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is empty", nameof(value));
        }

        if (value.Length > 40)
        {
            throw new ArgumentException("Value cant be longer than 40 chars", nameof(value));
        }

        return new ProjectName(value);
    }

    public static ProjectName Empty()
    {
        return new ProjectName(string.Empty);
    }
}
