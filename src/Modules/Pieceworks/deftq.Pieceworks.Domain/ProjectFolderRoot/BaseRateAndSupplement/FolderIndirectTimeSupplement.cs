using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public sealed class FolderIndirectTimeSupplement : ValueObject
    {
        public decimal Value { get; private set; }
        public FolderValueInheritStatus InheritStatus { get; private set; }

        private FolderIndirectTimeSupplement()
        {
            Value = 0;
            InheritStatus = FolderValueInheritStatus.Overwrite();
        }

        private FolderIndirectTimeSupplement(decimal value, FolderValueInheritStatus inheritStatus)
        {
            if (value < 0)
            {
                throw new ArgumentException("Indirect time supplement must be greater than or equal to 0", nameof(value));
            }
            Value = value;
            InheritStatus = inheritStatus;
        }
        
        public static FolderIndirectTimeSupplement Create(decimal value, FolderValueInheritStatus inheritStatus)
        {
            return new FolderIndirectTimeSupplement(value, inheritStatus);
        }
        
        public static FolderIndirectTimeSupplement Empty()
        {
            return Inherit();
        }
        
        public static FolderIndirectTimeSupplement Inherit()
        {
            return new FolderIndirectTimeSupplement(0, FolderValueInheritStatus.Inherit());
        }
    }
}
