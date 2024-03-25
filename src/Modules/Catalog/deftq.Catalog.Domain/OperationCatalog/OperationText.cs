using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.OperationCatalog
{
    public sealed class OperationText : ValueObject
    {
        public string Value { get; private set; }
        
        private OperationText()
        {
            Value = string.Empty;
        }

        private OperationText(string value)
        {
            Value = value;
        }

        public static OperationText Create(string value)
        {
            return new OperationText(value);
        }
        
        public static OperationText Empty()
        {
            return new OperationText(string.Empty);
        }
    }
}
