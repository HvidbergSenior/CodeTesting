using System.Net.Mail;
using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.project
{
    public sealed class ProjectEmail : ValueObject
    {
        public string Value { get; private set; }

        private ProjectEmail()
        {
            Value = String.Empty;
        }

        private ProjectEmail(string value)
        {
            if (!IsValidEmail(value))
            {
                throw new FormatException("Invalid email format");
            }
            Value = value;
        }

        public static ProjectEmail Create(string email)
        {
            return new ProjectEmail(email);
        }

        public static ProjectEmail Empty()
        {
            return new ProjectEmail();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAdr = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
