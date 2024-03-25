
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCompensation
{
    public class RegisterCompensationRequest
    {
        public decimal CompensationPayment { get; private set; }
        public DateOnly CompensationStartDate { get; private set; }
        public DateOnly CompensationEndDate { get; private set; }
        public IList<Guid> CompensationParticipantIds { get; private set; }

        public RegisterCompensationRequest(decimal compensationPayment, DateOnly compensationStartDate,
            DateOnly compensationEndDate, IList<Guid> compensationParticipantIds)
        {
            CompensationPayment = compensationPayment;
            CompensationStartDate = compensationStartDate;
            CompensationEndDate = compensationEndDate;
            CompensationParticipantIds = compensationParticipantIds;
        }
    }
}
