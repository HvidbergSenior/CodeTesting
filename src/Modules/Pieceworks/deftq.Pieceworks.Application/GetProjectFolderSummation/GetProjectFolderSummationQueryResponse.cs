namespace deftq.Pieceworks.Application.GetProjectFolderSummation
{
    public class GetProjectFolderSummationQueryResponse
    {
        public decimal TotalWorkTimeMilliseconds { get; private set; }
        public decimal TotalPaymentDkr { get; private set; }

        public decimal TotalExtraWorkTimeMilliseconds { get; private set; }
        public decimal TotalExtraPaymentDkr { get; private set; }

        public GetProjectFolderSummationQueryResponse(decimal totalWorkTimeMilliseconds, decimal totalPaymentDkr,
            decimal totalExtraWorkTimeMilliseconds, decimal totalExtraPaymentDkr)
        {
            TotalWorkTimeMilliseconds = totalWorkTimeMilliseconds;
            TotalPaymentDkr = totalPaymentDkr;
            TotalExtraWorkTimeMilliseconds = totalExtraWorkTimeMilliseconds;
            TotalExtraPaymentDkr = totalExtraPaymentDkr;
        }
    }
}
