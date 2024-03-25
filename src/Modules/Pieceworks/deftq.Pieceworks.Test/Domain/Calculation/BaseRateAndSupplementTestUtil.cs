using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;

namespace deftq.Pieceworks.Test.Domain.Calculation
{
    public static class BaseRateAndSupplementTestUtil
    {
        internal static Pieceworks.Domain.BaseRateAndSupplement GetDefaultBaseRateAndSupplement()
        {
            return CreateBaseRateAndSupplement(64, 2, 6, 214.75m, 0);
        }

        internal static Pieceworks.Domain.BaseRateAndSupplement CreateBaseRateAndSupplement(decimal indirectTime, decimal siteSpecific,
            decimal personalTime, decimal baseRate, decimal baseRateRegulation)
        {
            var indirectTimeSupplement = IndirectTimeSupplement.Create(indirectTime);
            var siteSpecificTimeSupplement = SiteSpecificTimeSupplement.Create(siteSpecific);
            var personalTimeSupplementInterval =
                PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(personalTime), DateInterval.Always());
            var personalTimeSupplement = new List<PersonalTimeSupplementInterval> { personalTimeSupplementInterval };
            var baseRateValue = new List<BaseRateInterval> { BaseRateInterval.Create(BaseRate.Create(baseRate), DateInterval.Always()) };
            var baseRateRegulationValue = BaseRateRegulation.Create(baseRateRegulation);

            return Pieceworks.Domain.BaseRateAndSupplement.Create(indirectTimeSupplement, siteSpecificTimeSupplement,
                personalTimeSupplement, baseRateValue, baseRateRegulationValue);
        }

        internal static FolderRateAndSupplement GetDefaultFolderRateAndSupplement()
        {
            return FolderRateAndSupplement.Create(GetDefaultBaseRateAndSupplement());
        }

        internal static BaseRateAndSupplementProxy GetDefaultBaseRateAndSupplementProxy()
        {
            var baseRateAndSupplement = GetDefaultBaseRateAndSupplement();
            return new BaseRateAndSupplementProxy(baseRateAndSupplement, Any.ProjectFolder(FolderRateAndSupplement.Create(baseRateAndSupplement)));
        }

        internal static WorkItemCalculator GetDefaultWorkItemCalculator()
        {
            return new WorkItemCalculator(GetDefaultBaseRateAndSupplementProxy());
        }
    }
}
