using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.SupplementCatalog
{
    public sealed class SupplementText : ValueObject
    {
        public string Value { get; private set; }

        private SupplementText()
        {
            Value = string.Empty;
        }

        private SupplementText(string value)
        {
            Value = value;
        }

        public static SupplementText Create(string value)
        {
            return new SupplementText(value);
        }
        
        public static SupplementText Empty()
        {
            return new SupplementText(string.Empty);
        }
    }
}
