using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class CatalogFavoriteId : ValueObject
    {
        public Guid Value { get; private set; }

        private CatalogFavoriteId()
        {
            Value = Guid.Empty;
        }

        private CatalogFavoriteId(Guid value)
        {
            Value = value;
        }

        public static CatalogFavoriteId Create(Guid value)
        {
            return new CatalogFavoriteId(value);
        }

        public static CatalogFavoriteId Empty()
        {
            return new CatalogFavoriteId();
        }
    }
}
