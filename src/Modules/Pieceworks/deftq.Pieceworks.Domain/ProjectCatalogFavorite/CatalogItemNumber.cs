using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class CatalogItemNumber : ValueObject
    {
        public String Value { get; private set; }

        private CatalogItemNumber()
        {
            Value = String.Empty;
        }

        private CatalogItemNumber(String value)
        {
            Value = value;
        }

        public static CatalogItemNumber Create(String value)
        {
            return new CatalogItemNumber(value);
        }

        public static CatalogItemNumber Empty()
        {
            return new CatalogItemNumber();
        }
    }
}
