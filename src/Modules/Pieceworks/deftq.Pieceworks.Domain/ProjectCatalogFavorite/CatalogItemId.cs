using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class CatalogItemId : ValueObject
    {
        public Guid Value { get; private set; }

        private CatalogItemId()
        {
            Value = Guid.Empty;
        }

        private CatalogItemId(Guid value)
        {
            Value = value;
        }

        public static CatalogItemId Create(Guid value)
        {
            return new CatalogItemId(value);
        }

        public static CatalogItemId Empty()
        {
            return new CatalogItemId();
        }
    }
}
