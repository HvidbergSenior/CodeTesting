using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.OperationCatalog
{
    public sealed class OperationNumber : ValueObject
    {
        public String Value { get; private set; }

        private OperationNumber()
        {
            Value = String.Empty;
        }

        private OperationNumber(string value)
        {
            Value = value;
        }

        public static OperationNumber Create(string value)
        {
            return new OperationNumber(value);
        }

        public static OperationNumber Empty()
        {
            return new OperationNumber(string.Empty);
        }
    }
}
