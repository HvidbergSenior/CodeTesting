namespace deftq.Pieceworks.Application.GetProjectCompensation
{
    public class GetProjectCompensationListQueryResponse
    {
        public IList<CompensationResponse> Compensations { get; private set; }

        private GetProjectCompensationListQueryResponse()
        {
            Compensations = new List<CompensationResponse>();
        }

        public GetProjectCompensationListQueryResponse(IList<CompensationResponse> compensations)
        {
            Compensations = compensations;
        }
    }

    public class CompensationResponse
    {
        public Guid ProjectCompensationId { get; }
        public DateOnly StartDate { get; }
        public DateOnly EndDate { get; }
        public decimal CompensationPaymentDkr { get; }
        public IList<ProjectCompensationParticipant> CompensationParticipant { get; }

        public CompensationResponse(Guid projectCompensationId, DateOnly startDate, DateOnly endDate, decimal compensationPaymentDkr,
            IList<ProjectCompensationParticipant> compensationParticipant)
        {
            ProjectCompensationId = projectCompensationId;
            StartDate = startDate;
            EndDate = endDate;
            CompensationPaymentDkr = compensationPaymentDkr;
            CompensationParticipant = compensationParticipant;
        }
    }

    public class ProjectCompensationParticipant
    {
        public Guid CompensationParticipantId { get; }
        public string ParticipantName { get; }
        public string ParticipantEmail { get; }
        public decimal ClosedHoursInPeriod { get; }
        public decimal CompensationAmountDkr { get; }

        private ProjectCompensationParticipant()
        {
            CompensationParticipantId = Guid.Empty;
            ParticipantName = string.Empty;
            ParticipantEmail = string.Empty;
            ClosedHoursInPeriod = -1;
            CompensationAmountDkr = -1;
        }

        public ProjectCompensationParticipant(Guid compensationParticipantId, string participantName, string participantEmail,
            decimal closedHoursInPeriod, decimal compensationAmountDkr)
        {
            CompensationParticipantId = compensationParticipantId;
            ParticipantName = participantName;
            ParticipantEmail = participantEmail;
            ClosedHoursInPeriod = closedHoursInPeriod;
            CompensationAmountDkr = compensationAmountDkr;
        }
    }
}
