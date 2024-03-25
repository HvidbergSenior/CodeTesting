namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetCompensationPaymentUsers
{
    public class GetCompensationPaymentParticipantsInPeriodRequest
    {
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal Amount { get; set; }

        public GetCompensationPaymentParticipantsInPeriodRequest(DateOnly startDate, DateOnly endDate, decimal amount)
        {
            StartDate = startDate;
            EndDate = endDate;
            Amount = amount;
        }
    }
}
