using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class SupplementOperationId : ValueObject
    {
        public Guid Value { get; private set; }

        private SupplementOperationId()
        {
            Value = Guid.Empty;
        }

        private SupplementOperationId(Guid value)
        {
            Value = value;
        }

        public static SupplementOperationId Create(Guid value)
        {
            return new SupplementOperationId(value);
        }
        
        public static SupplementOperationId Empty()
        {
            return new SupplementOperationId(Guid.Empty);
        }
    }
}
