using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.OperationCatalog
{
    public sealed class OperationId : ValueObject
    {
        public Guid Value { get; private set; }

        private OperationId()
        {
            Value = Guid.Empty;
        }

        private OperationId(Guid value)
        {
            Value = value;
        }

        public static OperationId Create(Guid value)
        {
            return new OperationId(value);
        }
        
        public static OperationId Empty()
        {
            return new OperationId(Guid.Empty);
        }
    }
}
