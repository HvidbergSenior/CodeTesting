using System.Net.Mail;
using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Validation.Webapi.Messages;

namespace deftq.Pieceworks.Domain.InvitationFlow
{
    public sealed class InvitationEmail : ValueObject
    {
        public string Value { get; private set; }

        private InvitationEmail()
        {
            Value = string.Empty;
        }

        private InvitationEmail(string value)
        {
            Value = value;
        }

        public static InvitationEmail Create(string value)
        {
            var validEmail = MailAddress.TryCreate(value, out _);
            if (!validEmail)
            {
                throw new ArgumentException(ExceptionMessages.InvalidEmail(value), nameof(value));
            }
            return new InvitationEmail(value);
        }

        public static InvitationEmail Empty()
        {
            return new InvitationEmail(string.Empty);
        }
    }
}
