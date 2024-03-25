using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class MaterialPublished : ValueObject
    {
        public DateTimeOffset Value { get; private set; }

        private MaterialPublished()
        {
            Value = DateTimeOffset.MinValue;
        }

        private MaterialPublished(DateTimeOffset value)
        {
            Value = value;
        }

        public static MaterialPublished Create(DateTimeOffset value)
        {
            return new MaterialPublished(value);
        }
        
        public static MaterialPublished Empty()
        {
            return new MaterialPublished();
        }
    }
}
