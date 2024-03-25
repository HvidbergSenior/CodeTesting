using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectDocument;

public sealed class ProjectDocumentUploadedTimestamp : ValueObject
{
    public DateTimeOffset Value { get; private set; }

    private ProjectDocumentUploadedTimestamp()
    {
        Value = DateTimeOffset.MinValue;
    }

    private ProjectDocumentUploadedTimestamp(DateTimeOffset value)
    {
        Value = value;
    }

    public static ProjectDocumentUploadedTimestamp Create(DateTimeOffset value)
    {
        return new ProjectDocumentUploadedTimestamp(value);
    }

    public static ProjectDocumentUploadedTimestamp Empty()
    {
        return new ProjectDocumentUploadedTimestamp(DateTimeOffset.MinValue);
    }
}
