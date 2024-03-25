using deftq.Pieceworks.Domain.InvitationFlow;

namespace deftq.Pieceworks.Application.GetProjectInvitation
{
    public class InvitationResponse
    {
        public Guid InvitationId { get; }
        public Guid ProjectId { get; }
        public string Email { get; }
        public string? RandomValue { get; }
        public DateTime ExpirationDate { get; }
        public int Retries { get; }
        public bool EmailSent { get; }
        public InvitationStatus Status { get; }

        public InvitationResponse(Guid invitationId, Guid projectId, string email, string? randomValue, DateTime expirationDate, int retries, bool emailSent, InvitationStatus status)
        {
            InvitationId = invitationId;
            ProjectId = projectId;
            Email = email;
            RandomValue = randomValue;
            ExpirationDate = expirationDate;
            Retries = retries;
            EmailSent = emailSent;
            Status = status;
        }
    }
}
