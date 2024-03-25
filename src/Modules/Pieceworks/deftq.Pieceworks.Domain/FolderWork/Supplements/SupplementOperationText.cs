using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class SupplementOperationText : ValueObject
    {
        public string Value { get; private set; }

        private SupplementOperationText()
        {
            Value = string.Empty;
        }

        private SupplementOperationText(string value)
        {
            Value = value;
        }

        public static SupplementOperationText Create(string value)
        {
            return new SupplementOperationText(value);
        }
        
        public static SupplementOperationText Empty()
        {
            return new SupplementOperationText(string.Empty);
        }
    }
}
