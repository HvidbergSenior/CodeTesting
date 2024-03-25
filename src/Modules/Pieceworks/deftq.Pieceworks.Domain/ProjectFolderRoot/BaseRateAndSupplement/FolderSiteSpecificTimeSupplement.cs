using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public sealed class FolderSiteSpecificTimeSupplement : ValueObject
    {
        public decimal Value { get; private set; }
        public FolderValueInheritStatus InheritStatus { get; private set; }

        private FolderSiteSpecificTimeSupplement()
        {
            Value = 0;
            InheritStatus = FolderValueInheritStatus.Overwrite();
        }

        private FolderSiteSpecificTimeSupplement(decimal value, FolderValueInheritStatus inheritStatus)
        {
            if (value < 0)
            {
                throw new ArgumentException("Site specific supplement must be greater than or equal to 0", nameof(value));
            }
            Value = value;
            InheritStatus = inheritStatus;
        }
        
        public static FolderSiteSpecificTimeSupplement Create(decimal value, FolderValueInheritStatus inheritStatus)
        {
            return new FolderSiteSpecificTimeSupplement(value, inheritStatus);
        }
        
        public static FolderSiteSpecificTimeSupplement Empty()
        {
            return Inherit();
        }
        
        public static FolderSiteSpecificTimeSupplement Inherit()
        {
            return new FolderSiteSpecificTimeSupplement(0, FolderValueInheritStatus.Inherit());
        }
    }
}
