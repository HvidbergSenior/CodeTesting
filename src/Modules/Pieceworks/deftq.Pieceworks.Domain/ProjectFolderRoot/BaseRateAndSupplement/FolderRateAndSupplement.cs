using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public sealed class FolderRateAndSupplement : ValueObject
    {
        public FolderIndirectTimeSupplement IndirectTimeSupplement { get; private set; }
        public FolderSiteSpecificTimeSupplement SiteSpecificTimeSupplement { get; private set; }
        public FolderBaseRateRegulation BaseRateRegulation { get; private set; }

        private FolderRateAndSupplement()
        {
            IndirectTimeSupplement = FolderIndirectTimeSupplement.Empty();
            SiteSpecificTimeSupplement = FolderSiteSpecificTimeSupplement.Empty();
            BaseRateRegulation = FolderBaseRateRegulation.Empty();
        }

        private FolderRateAndSupplement(FolderIndirectTimeSupplement indirectTimeSupplement,
            FolderSiteSpecificTimeSupplement siteSpecificTimeSupplement, FolderBaseRateRegulation baseRateRegulation)
        {
            IndirectTimeSupplement = indirectTimeSupplement;
            SiteSpecificTimeSupplement = siteSpecificTimeSupplement;
            BaseRateRegulation = baseRateRegulation;
        }

        public static FolderRateAndSupplement Create(FolderIndirectTimeSupplement indirectTimeSupplement,
            FolderSiteSpecificTimeSupplement siteSpecificTimeSupplement, FolderBaseRateRegulation baseRateRegulation)
        {
            return new FolderRateAndSupplement(indirectTimeSupplement, siteSpecificTimeSupplement, baseRateRegulation);
        }

        public static FolderRateAndSupplement Create(Domain.BaseRateAndSupplement baseRateAndSupplement)
        {
            var folderIndirectTimeSupplement =
                FolderIndirectTimeSupplement.Create(baseRateAndSupplement.IndirectTimeSupplement.Value, FolderValueInheritStatus.Inherit());
            var folderSiteSpecificTimeSupplement = FolderSiteSpecificTimeSupplement.Create(baseRateAndSupplement.SiteSpecificTimeSupplement.Value,
                FolderValueInheritStatus.Inherit());
            var folderBaseRateRegulation =
                FolderBaseRateRegulation.Create(baseRateAndSupplement.BaseRateRegulation.Value, FolderValueInheritStatus.Inherit());
            return new FolderRateAndSupplement(folderIndirectTimeSupplement, folderSiteSpecificTimeSupplement, folderBaseRateRegulation);
        }

        public static FolderRateAndSupplement Empty()
        {
            return InheritAll();
        }

        public static FolderRateAndSupplement InheritAll()
        {
            return new FolderRateAndSupplement(FolderIndirectTimeSupplement.Inherit(), FolderSiteSpecificTimeSupplement.Inherit(),
                FolderBaseRateRegulation.Inherit());
        }
        
        public static FolderRateAndSupplement OverwriteAll(Domain.BaseRateAndSupplement baseRateAndSupplement)
        {
            var folderIndirectTimeSupplement =
                FolderIndirectTimeSupplement.Create(baseRateAndSupplement.IndirectTimeSupplement.Value, FolderValueInheritStatus.Overwrite());
            var folderSiteSpecificTimeSupplement = FolderSiteSpecificTimeSupplement.Create(baseRateAndSupplement.SiteSpecificTimeSupplement.Value,
                FolderValueInheritStatus.Overwrite());
            var folderBaseRateRegulation =
                FolderBaseRateRegulation.Create(baseRateAndSupplement.BaseRateRegulation.Value, FolderValueInheritStatus.Overwrite());
            return new FolderRateAndSupplement(folderIndirectTimeSupplement, folderSiteSpecificTimeSupplement, folderBaseRateRegulation);
        }
    }
}
