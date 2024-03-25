using System.Net.Mail;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectInvitation
{
    public class CreateInvitationRequest
    {
        public string Email { get; }
        public CreateInvitationRequest(string email)
        {
            Email = email;
        }
    }
}
