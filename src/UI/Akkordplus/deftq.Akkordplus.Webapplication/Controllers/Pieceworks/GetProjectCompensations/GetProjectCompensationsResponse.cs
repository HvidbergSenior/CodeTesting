
namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectCompensations
{
    public class GetProjectCompensationsResponse
    {
        public IList<CompensationResponse> Compensations { get; private set; }

        private GetProjectCompensationsResponse()
        {
            Compensations = new List<CompensationResponse>();
        }

        public GetProjectCompensationsResponse(IList<CompensationResponse> compensations)
        {
            Compensations = compensations;
        }
    }

    public class CompensationResponse
    {
        public Guid CompensationId { get; private set; }
        public CompensationPeriod CompensationPeriod { get; private set; }
        public IList<CompensationParticipantId> CompensationParticipantIds { get; private set; }
        public decimal CompensationPayment { get; private set; }

        public CompensationResponse(Guid compensationId, CompensationPeriod compensationPeriod,
            IList<CompensationParticipantId> compensationParticipantIds, decimal compensationPayment)
        {
            CompensationId = compensationId;
            CompensationPeriod = compensationPeriod;
            CompensationParticipantIds = compensationParticipantIds;
            CompensationPayment = compensationPayment;
        }
    }
    
    public class CompensationPeriod
    {
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }

        private CompensationPeriod()
        {
            StartDate = DateOnly.MinValue;
            EndDate = DateOnly.MaxValue;
        }

        public CompensationPeriod(DateOnly startDate, DateOnly endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public class CompensationParticipantId
    {
        public Guid ParticipantId { get; private set; }

        private CompensationParticipantId()
        {
            ParticipantId = Guid.Empty;
        }

        public CompensationParticipantId(Guid participantId)
        {
            ParticipantId = participantId;
        }
    }
}
