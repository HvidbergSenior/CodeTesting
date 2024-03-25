using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class CatalogOperationId : ValueObject
    {
        public Guid Value { get; private set; }

        private CatalogOperationId()
        {
            Value = Guid.Empty;
        }

        private CatalogOperationId(Guid value)
        {
            Value = value;
        }

        public static CatalogOperationId Create(Guid value)
        {
            return new CatalogOperationId(value);
        }

        public static CatalogOperationId Empty()
        {
            return new CatalogOperationId(Guid.Empty);
        }
    }
}
