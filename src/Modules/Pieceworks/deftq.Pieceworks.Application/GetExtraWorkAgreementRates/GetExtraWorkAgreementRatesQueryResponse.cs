namespace deftq.Pieceworks.Application.GetExtraWorkAgreementRates
{
    public class GetExtraWorkAgreementRatesQueryResponse
    {
        public decimal CustomerRatePerHourDkr { get; private set; }
        public decimal CompanyRatePerHourDkr { get; private set; }

        public GetExtraWorkAgreementRatesQueryResponse(decimal customerRatePerHourDkr, decimal companyRatePerHourDkr)
        {
            CustomerRatePerHourDkr = customerRatePerHourDkr;
            CompanyRatePerHourDkr = companyRatePerHourDkr;
        }
    }
}
