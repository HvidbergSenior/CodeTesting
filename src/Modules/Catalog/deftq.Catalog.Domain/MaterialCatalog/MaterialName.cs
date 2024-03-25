using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class MaterialName : ValueObject
    {
        public String Value { get; private set; }

        private MaterialName()
        {
            Value = String.Empty;
        }

        private MaterialName(string value)
        {
            Value = value;
        }

        public static MaterialName Create(string value)
        {
            return new MaterialName(value);
        }
        
        public static MaterialName Empty()
        {
            return new MaterialName(string.Empty);
        }
    }
}
