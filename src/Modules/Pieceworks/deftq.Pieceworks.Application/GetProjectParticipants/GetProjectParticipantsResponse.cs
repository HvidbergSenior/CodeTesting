using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Application.GetProjectInvitation;
using deftq.Pieceworks.Domain.InvitationFlow;

namespace deftq.Pieceworks.Application.GetProjectParticipants
{
    public class GetProjectParticipantsResponse
    {
        public IEnumerable<GetProjectParticipantResponse> Participants { get; }

        public GetProjectParticipantsResponse(IEnumerable<GetProjectParticipantResponse> participants)
        {
            Participants = participants;
        }
    }

    public class GetProjectParticipantResponse
    {
        public string Name { get; }
        public ProjectParticipantRole Role { get; }
        public string Email { get; }
        public InvitationResponse? Invitation { get; private set; }

        public GetProjectParticipantResponse(string name, ProjectParticipantRole role, string email, InvitationResponse? invitation = null)
        {
            Name = name;
            Role = role;
            Email = email;
            Invitation = invitation;
        }

        public void SetParticipantInvitation(Invitation? invitation)
        {
            if (invitation == null || string.Equals(invitation.Email.Value, Email, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Invitation = new InvitationResponse(invitation!.InvitationId.Value,
                invitation.ProjectId.Value,
                invitation.Email.Value,
                invitation.RandomValue.Value,
                invitation.ExpirationDate.Value,
                invitation.Retries.Value,
                invitation.EmailSent,
                invitation.Status);
        }
    }

    public enum ProjectParticipantRole
    {
        ProjectManager,
        ProjectOwner,
        ProjectParticipant,
        ProjectApprentice,
        Undefined
    }
}
