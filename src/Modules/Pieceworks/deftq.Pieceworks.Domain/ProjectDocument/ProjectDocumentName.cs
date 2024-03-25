using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectDocument;

public sealed class ProjectDocumentName : ValueObject
{
    public String Value { get; private set; }

    private ProjectDocumentName()
    {
        Value = string.Empty;
    }

    private ProjectDocumentName(string value)
    {
        Value = value;
    }

    public static ProjectDocumentName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is empty", nameof(value));
        }

        if (value.Length > 40)
        {
            throw new ArgumentException("Value cant be longer than 40 chars", nameof(value));
        }

        return new ProjectDocumentName(value);
    }

    public static ProjectDocumentName Empty()
    {
        return new ProjectDocumentName(string.Empty);
    }
}
