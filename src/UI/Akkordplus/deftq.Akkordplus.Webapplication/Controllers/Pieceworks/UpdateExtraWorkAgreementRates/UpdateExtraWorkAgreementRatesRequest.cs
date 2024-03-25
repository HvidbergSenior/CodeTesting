namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateExtraWorkAgreementRates
{
    public class UpdateExtraWorkAgreementRatesRequest
    {
        public UpdateExtraWorkAgreementRatesRequest(decimal customerRatePrHour, decimal companyRatePrHour)
        {
            CustomerRatePrHour = customerRatePrHour;
            CompanyRatePrHour = companyRatePrHour;
        }
        public decimal CustomerRatePrHour { get; set; }
        public decimal CompanyRatePrHour { get; set; }

    }
}
