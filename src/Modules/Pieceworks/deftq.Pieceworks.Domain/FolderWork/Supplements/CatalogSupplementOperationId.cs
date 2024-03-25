using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class CatalogSupplementOperationId : ValueObject
    {
        public Guid Value { get; private set; }

        private CatalogSupplementOperationId()
        {
            Value = Guid.Empty;
        }

        private CatalogSupplementOperationId(Guid value)
        {
            Value = value;
        }

        public static CatalogSupplementOperationId Create(Guid value)
        {
            return new CatalogSupplementOperationId(value);
        }

        public static CatalogSupplementOperationId Empty()
        {
            return new CatalogSupplementOperationId(Guid.Empty);
        }
    }
}
