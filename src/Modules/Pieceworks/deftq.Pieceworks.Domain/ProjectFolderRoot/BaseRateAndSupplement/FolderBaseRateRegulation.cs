using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public sealed class FolderBaseRateRegulation : ValueObject
    {
        public decimal Value { get; private set; }
        public FolderValueInheritStatus InheritStatus { get; private set; }

        private FolderBaseRateRegulation()
        {
            Value = 0;
            InheritStatus = FolderValueInheritStatus.Overwrite();
        }

        private FolderBaseRateRegulation(decimal value, FolderValueInheritStatus inheritStatus)
        {
            if (value < 0)
            {
                throw new ArgumentException("Base rate regulation must be greater than or equal to 0", nameof(value));
            }
            Value = value;
            InheritStatus = inheritStatus;
        }
        
        public static FolderBaseRateRegulation Create(decimal value, FolderValueInheritStatus inheritStatus)
        {
            return new FolderBaseRateRegulation(value, inheritStatus);
        }
        
        public static FolderBaseRateRegulation Empty()
        {
            return Inherit();
        }
        
        public static FolderBaseRateRegulation Inherit()
        {
            return new FolderBaseRateRegulation(0, FolderValueInheritStatus.Inherit());
        }
    }
}
