using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.ProjectCatalogFavorite
{
    public sealed class CatalogItemText : ValueObject
    {
        public String Value { get; private set; }

        private CatalogItemText()
        {
            Value = String.Empty;
        }

        private CatalogItemText(String value)
        {
            Value = value;
        }

        public static CatalogItemText Create(String value)
        {
            return new CatalogItemText(value);
        }

        public static CatalogItemText Empty()
        {
            return new CatalogItemText();
        }
    }
}
