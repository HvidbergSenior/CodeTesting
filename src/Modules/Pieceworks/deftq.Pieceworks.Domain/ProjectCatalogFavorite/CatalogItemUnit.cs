using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class CatalogItemUnit : ValueObject
    {
        public String Value { get; private set; }

        private CatalogItemUnit()
        {
            Value = String.Empty;
        }

        private CatalogItemUnit(String value)
        {
            Value = value;
        }

        public static CatalogItemUnit Create(String value)
        {
            return new CatalogItemUnit(value);
        }

        public static CatalogItemUnit Empty()
        {
            return new CatalogItemUnit();
        }
    }
}
