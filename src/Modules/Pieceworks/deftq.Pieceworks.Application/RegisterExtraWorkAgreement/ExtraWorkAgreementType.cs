using deftq.Pieceworks.Domain.projectExtraWorkAgreement;

namespace deftq.Pieceworks.Application.RegisterExtraWorkAgreement
{
    public enum ExtraWorkAgreementType
    {
        CustomerHours,
        CompanyHours,
        AgreedPayment,
        Other
    }

    public static class MapToDomain
    {
        public static ProjectExtraWorkAgreementType ToDomain(this ExtraWorkAgreementType extraWorkAgreementType)
        {
            return extraWorkAgreementType switch
            {
                ExtraWorkAgreementType.CustomerHours => ProjectExtraWorkAgreementType.CustomerHours(),
                ExtraWorkAgreementType.CompanyHours => ProjectExtraWorkAgreementType.CompanyHours(),
                ExtraWorkAgreementType.AgreedPayment => ProjectExtraWorkAgreementType.AgreedPayment(),
                ExtraWorkAgreementType.Other => ProjectExtraWorkAgreementType.Other(),
                _ => throw new ArgumentOutOfRangeException(extraWorkAgreementType.GetType().Name),
            };
        }
    }
}
