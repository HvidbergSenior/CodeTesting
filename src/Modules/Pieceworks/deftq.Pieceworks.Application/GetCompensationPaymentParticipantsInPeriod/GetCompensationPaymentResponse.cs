namespace deftq.Pieceworks.Application.GetCompensationPaymentParticipantsInPeriod
{
    public class GetCompensationPaymentResponse
    {
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public IEnumerable<GetCompensationPaymentParticipantResponse> Participants { get; private set; }

        public GetCompensationPaymentResponse(DateOnly startDate, DateOnly endDate,
            IEnumerable<GetCompensationPaymentParticipantResponse> participants)
        {
            StartDate = startDate;
            EndDate = endDate;
            Participants = participants;
        }
    }

    public class GetCompensationPaymentParticipantResponse
    {
        public Guid ProjectParticipantId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public decimal Hours { get; private set; }
        public decimal Payment { get; private set; }

        public GetCompensationPaymentParticipantResponse(Guid projectParticipantId, string name, string email, decimal hours, decimal payment)
        {
            ProjectParticipantId = projectParticipantId;
            Name = name;
            Email = email;
            Hours = hours;
            Payment = payment;
        }
    }
}
