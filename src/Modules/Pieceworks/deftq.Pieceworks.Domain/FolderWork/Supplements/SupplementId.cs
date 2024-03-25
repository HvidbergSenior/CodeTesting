using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class SupplementId : ValueObject
    {
        public Guid Value { get; private set; }

        private SupplementId()
        {
            Value = Guid.Empty;
        }

        private SupplementId(Guid value)
        {
            Value = value;
        }

        public static SupplementId Create(Guid value)
        {
            return new SupplementId(value);
        }

        public static SupplementId Empty()
        {
            return new SupplementId(Guid.Empty);
        }
    }
}
