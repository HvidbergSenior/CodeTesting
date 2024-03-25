namespace deftq.Pieceworks.Application.GetProjectSummation
{
    public class GetProjectSummationQueryResponse
    {
        public decimal TotalWorkItemPaymentDkr { get; private set; }
        public decimal TotalWorkItemExtraWorkPaymentDkr { get; private set; }
        public decimal TotalExtraWorkAgreementDkr { get; private set; }
        public decimal TotalLogBookHours { get; private set; }
        public decimal TotalPaymentDkr { get; private set; }
        public decimal TotalLumpSumDkr { get; private set; }
        public decimal TotalCalculationSumDkr { get; private set; }

        public GetProjectSummationQueryResponse(decimal totalWorkItemPaymentDkr, decimal totalWorkItemExtraWorkPaymentDkr,
            decimal totalExtraWorkAgreementDkr, decimal totalLogBookHours, decimal totalPaymentDkr, decimal totalLumpSumDkr,
            Decimal totalCalculationSumDkr)
        {
            TotalWorkItemPaymentDkr = totalWorkItemPaymentDkr;
            TotalWorkItemExtraWorkPaymentDkr = totalWorkItemExtraWorkPaymentDkr;
            TotalExtraWorkAgreementDkr = totalExtraWorkAgreementDkr;
            TotalLogBookHours = totalLogBookHours;
            TotalPaymentDkr = totalPaymentDkr;
            TotalLumpSumDkr = totalLumpSumDkr;
            TotalCalculationSumDkr = totalCalculationSumDkr;
        }
    }
}
