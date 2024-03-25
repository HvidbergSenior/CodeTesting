using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectDocument
{
    public sealed class ProjectDocumentId : ValueObject
    {
        public Guid Value { get; private set; }

        private ProjectDocumentId()
        {
            Value = Guid.Empty;
        }

        private ProjectDocumentId(Guid value)
        {
            Value = value;
        }

        public static ProjectDocumentId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(id));
            }

            return new ProjectDocumentId(id);
        }

        public static ProjectDocumentId Empty()
        {
            return new ProjectDocumentId(Guid.Empty);
        }
    }
}
