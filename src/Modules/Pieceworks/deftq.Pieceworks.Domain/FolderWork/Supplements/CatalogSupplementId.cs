using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class CatalogSupplementId : ValueObject
    {
        public Guid Value { get; private set; }

        private CatalogSupplementId()
        {
            Value = Guid.Empty;
        }

        private CatalogSupplementId(Guid value)
        {
            Value = value;
        }

        public static CatalogSupplementId Create(Guid value)
        {
            return new CatalogSupplementId(value);
        }

        public static CatalogSupplementId Empty()
        {
            return new CatalogSupplementId(Guid.Empty);
        }
    }
}
