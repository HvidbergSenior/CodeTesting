using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class MaterialId : ValueObject
    {
        public Guid Value { get; private set; }

        private MaterialId()
        {
            Value = Guid.Empty;
        }

        private MaterialId(Guid value)
        {
            Value = value;
        }

        public static MaterialId Create(Guid value)
        {
            return new MaterialId(value);
        }
        
        public static MaterialId Empty()
        {
            return new MaterialId(Guid.Empty);
        }
    }
}
