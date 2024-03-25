namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveCompensationPayments
{
    
    public class RemoveCompensationPaymentsRequest
    {
        public IList<Guid> CompensationPaymentIds { get; }

        public RemoveCompensationPaymentsRequest(IList<Guid> compensationPaymentIds)
        {
            CompensationPaymentIds = compensationPaymentIds;
        }
    }
}
