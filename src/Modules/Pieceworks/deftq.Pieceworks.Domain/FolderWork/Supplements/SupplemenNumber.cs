using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class SupplementNumber : ValueObject
    {
        public string Value { get; private set; }

        private SupplementNumber()
        {
            Value = string.Empty;
        }

        private SupplementNumber(string value)
        {
            Value = value;
        }

        public static SupplementNumber Create(string value)
        {
            return new SupplementNumber(value);
        }

        public static SupplementNumber Empty()
        {
            return new SupplementNumber(string.Empty);
        }
    }
}
