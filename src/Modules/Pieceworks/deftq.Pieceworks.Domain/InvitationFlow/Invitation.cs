using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public sealed class Invitation : Entity
    {
        public InvitationId InvitationId { get; private set; }
        public ProjectId ProjectId { get; private set; }
        public InvitationEmail Email { get; private set; }
        public InvitationRandomValue RandomValue { get; private set; }
        public InvitationExpiration ExpirationDate { get; private set; }
        public InvitationRetries Retries { get; private set; }
        public bool EmailSent { get; private set; }

        public void SetEmailHasBeenSent()
        {
            EmailSent = true;
        }

        public InvitationStatus Status { get; private set; }

        public void SetStatus(InvitationStatus status)
        {
            Status = status;
        }

        private Invitation()
        {
            InvitationId = InvitationId.Empty();
            ProjectId = ProjectId.Empty();
            Email = InvitationEmail.Empty();
            RandomValue = InvitationRandomValue.NewValue();
            ExpirationDate = InvitationExpiration.Default();
            Retries = InvitationRetries.Empty();
            EmailSent = false;
            Status = InvitationStatus.Created;
        }

        private Invitation(InvitationId invitationId, ProjectId projectId, InvitationEmail email, InvitationRandomValue randomValue, InvitationExpiration expirationDate,
            InvitationRetries retries, bool emailSent = false, InvitationStatus status = InvitationStatus.Created)
        {
            Id = invitationId.Value;
            InvitationId = invitationId;
            ProjectId = projectId;
            Email = email;
            RandomValue = randomValue;
            ExpirationDate = expirationDate;
            Retries = retries;
            EmailSent = emailSent;
            Status = status;
        }

        public static Invitation Create(InvitationId invitationId,  ProjectId projectId, InvitationEmail email, InvitationRandomValue randomValue,
            InvitationExpiration expirationDate, InvitationRetries retries, bool emailSent = false,
            InvitationStatus status = InvitationStatus.Created)
        {
            return new Invitation(invitationId, projectId, email, randomValue, expirationDate, retries, emailSent, status);
        }
    }
}
