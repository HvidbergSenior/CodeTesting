using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Application.UpdateExtraWorkAgreement
{
    
    public enum UpdateExtraWorkAgreementType
    {
        CustomerHours,
        CompanyHours,
        AgreedPayment,
        Other
    }
    
    public static class MapToDomain
    {
        public static ProjectExtraWorkAgreementType ToDomain(this UpdateExtraWorkAgreementType updateExtraWorkAgreementType)
        {
            return updateExtraWorkAgreementType switch
            {
                UpdateExtraWorkAgreementType.CustomerHours => ProjectExtraWorkAgreementType.CustomerHours(),
                UpdateExtraWorkAgreementType.CompanyHours => ProjectExtraWorkAgreementType.CompanyHours(),
                UpdateExtraWorkAgreementType.AgreedPayment => ProjectExtraWorkAgreementType.AgreedPayment(),
                UpdateExtraWorkAgreementType.Other => ProjectExtraWorkAgreementType.Other(),
                _ => throw new ArgumentOutOfRangeException(updateExtraWorkAgreementType.GetType().Name),
            };
        }
    }
}
