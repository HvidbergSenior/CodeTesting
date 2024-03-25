using System.Collections.ObjectModel;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Domain.Calculation
{
    public class BaseRateAndSupplementProxy
    {
        private readonly BaseRateAndSupplement _systemBaseRateAndSupplement;
        private readonly FolderRateAndSupplement _folderRateAndSupplement;
        private readonly IList<FolderSupplement> _folderSupplements;

        public BaseRateAndSupplementProxy(BaseRateAndSupplement systemBaseRateAndSupplement, ProjectFolder destinationFolder)
        {
            _systemBaseRateAndSupplement = systemBaseRateAndSupplement;
            _folderRateAndSupplement = destinationFolder.FolderRateAndSupplement;
            _folderSupplements = destinationFolder.GetEffectiveFolderSupplements();
        }

        public decimal IndirectTimeSupplement
        {
            get
            {
                return _folderRateAndSupplement.IndirectTimeSupplement.Value;
            }
        }

        public decimal SiteSpecificTimeSupplement
        {
            get
            {
                return _folderRateAndSupplement.SiteSpecificTimeSupplement.Value;
            }
        }

        public decimal BaseRateRegulation
        {
            get
            {
                return _folderRateAndSupplement.BaseRateRegulation.Value;
            }
        }

        public IReadOnlyList<FolderSupplement> FolderSupplements
        {
            get
            {
                return new ReadOnlyCollection<FolderSupplement>(_folderSupplements);
            }
        }

        public decimal GetBaseRate(DateOnly day)
        {
            return _systemBaseRateAndSupplement.BaseRateIntervals.First(d => d.IsApplicable(day)).BaseRate.Value;
        }

        public decimal GetPersonalTimeSupplement(DateOnly day)
        {
            return _systemBaseRateAndSupplement.PersonalTimeSupplementIntervals.First(d => d.IsApplicable(day)).PersonalTimeSupplement.Value;
        }
    }
}
